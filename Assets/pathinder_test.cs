using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class pathinder_test : MonoBehaviour
{

    // pathinding
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachendofPath = false;
    public float speed;
    Rigidbody2D rb2;

    public Animator Eanimator;

    private bool coolDown;

    public Transform Target;

    public Transform ennemyGRX;


    Seeker seeker;
    
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();

        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

     void UpdatePath()
    {
        if (seeker.IsDone())
        seeker.StartPath(rb2.position, Target.position, OnpathComplete);
    }

    void OnpathComplete(Path p)
    {
        if(! p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        speed = 0f;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        speed = 20f;
        Eanimator.SetTrigger("Idle");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (!coolDown)
            {
                coolDown = true;
                StartCoroutine(CoolDown(collision));
            }

        }
    }
    //call it in the update !!
    private void Targetoutofrange()
    {
        if (Vector3.Distance(ennemyGRX.position, Target.position) < 2f)
        {
            reachendofPath = true;
        }
        else 
        {
            reachendofPath = false;
        }
    }

    private void Update()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachendofPath = true;
            return;
        }
 
        else
        {
            reachendofPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb2.velocity = force;

       

        float distance = Vector2.Distance(rb2.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) 
        {
            currentWaypoint++;
        }


        
        if (rb2.velocity.x <= 0.01)       
        {
            ennemyGRX.localScale = new Vector3(-1f, 1, 1f);
        }
        if (rb2.velocity.x >= -0.01)
        {
            ennemyGRX.localScale = new Vector3(1f, 1, 1f);
        }
       
        
    }
    IEnumerator CoolDown(Collision2D collision)
    {
        Eanimator.SetTrigger("Attacking");
        yield return new WaitForSeconds(1);
        //Debug.Log("enemy hit " + collision.gameObject.name + Time.time);
        //GetComponent<Enemy>().takeDamage(EnnemyDamage);
        coolDown = false;
    }
}
