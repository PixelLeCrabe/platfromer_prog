using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyPathfinder : MonoBehaviour
{
    // pathinding
    public float nextWaypointDistance = 5f;

    Path path;
    int currentWaypoint = 0;
    bool reachendofPath = false;
    public float speed;
    public float AttacKCD;
    public float recalculatingDelay = 1f;
    public Rigidbody2D rb2;

    //use instade of OnColliderStay mdr...
    private bool IsTouching;

    public Animator Eanimator;

    public Transform Target;

    public Transform ennemyGRX;

    private Vector2 direction = Vector2.zero;
    Seeker seeker;
    
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();

        seeker = GetComponent<Seeker>();

        seeker.StartPath(rb2.position, Target.position, OnpathComplete);
        StartCoroutine(RecalculatingPath(recalculatingDelay));
        currentWaypoint = 0;
        
    }

    private void Update()
    {
        //Targetoutofrange();
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachendofPath = true;
            //currentWaypoint = 0;
            return;
        }
        else
        {
            //not workingk!
            //reachendofPath = false;
        }

        
        //Flip the Ennemy
        if (Target.localPosition.x < transform.localPosition.x) 
        {
            ennemyGRX.localScale = new Vector3(-1f, 1, 1f);
        }
        if (Target.localPosition.x > transform.localPosition.x)
        {
            ennemyGRX.localScale = new Vector3(1f, 1, 1f);
        }

        Eanimator.SetFloat("Speed", rb2.velocity.x);
        if (rb2.velocity.x > 0)
        {
            Eanimator.SetTrigger("Walking");

        }
    }

    void FixedUpdate()
    {
        if(path != null)
        {
            //print("path n'est pas nul " + currentWaypoint);
            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2.position).normalized;
            float distance = Vector2.Distance(rb2.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance && currentWaypoint < path.vectorPath.Count - 1)
            {
                currentWaypoint++;
            }
        }
        
        Vector2 force = direction * speed * Time.deltaTime;

        rb2.velocity = force;

    }
       
    #region PathfindingMethods
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

    //Not working !
    private void Targetoutofrange()
    {
        if (Vector3.Distance(ennemyGRX.position, Target.position) < 1f)
        {
            reachendofPath = true;
        }
        else
        {
            reachendofPath = false;
        }
    }

    #endregion

    private  void OnCollisionEnter2D(Collision2D collision)
    {
        speed = 0f;     
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        speed = 20f;
        Eanimator.SetTrigger("Walking");
        Eanimator.SetBool("Attacking2", false);
    }   

    IEnumerator RecalculatingPath(float delay)
    {
        yield return new WaitForSeconds(delay);
        seeker.StartPath(rb2.position, Target.position, OnpathComplete);
        StartCoroutine(RecalculatingPath(delay));
    }

    
   

}

