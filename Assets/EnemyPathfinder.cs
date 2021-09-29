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
    public float attackCooldown;
    public float recalculatingDelay = 1f;
    Rigidbody2D rb2;

    bool isLeft;

    public Animator Eanimator;

    private bool isInCooldown;

    public Transform Target;

    public Transform ennemyGRX;

    private Vector2 direction = Vector2.zero;
    Seeker seeker;
    
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();

        seeker = GetComponent<Seeker>();
        //InvokeRepeating("UpdatePath", 0f, 0.2f);
        //Eanimator.SetBool("Attacking", false);

        seeker.StartPath(rb2.position, Target.position, OnpathComplete);
        StartCoroutine(RecalculatingPath(recalculatingDelay));
        isInCooldown = true;
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



        if (rb2.velocity.x <= 0.42)
        {
            ennemyGRX.localScale = new Vector3(-1f, 1, 1f);
            isLeft = true;
        }
        if (rb2.velocity.x >= -0.20)
        {
            ennemyGRX.localScale = new Vector3(1f, 1, 1f);
            isLeft = false;
        }

        print(rb2.velocity.x);
        print(isLeft);

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
            print("path n'est pas nul " + currentWaypoint);
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

    public void Attack()
    {
        //speed = 0f;
        if (!isInCooldown)
        {
            Eanimator.SetTrigger("Attacking");
            isInCooldown = false;
        }
    }

    public void PlayerDistanceCheck()
    {
        //D'abord on raycast pour voir si le player est siffusamment proche pour s'arrêter

        //Si c'est le cas, on s'arrête

        //Ensuite, on essaye d'attaquer

        //Si le player s'éloigne trop, on redémarre la speed
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       

        //if (collision.transform.CompareTag("Player"))
        //{
        //if (!coolDown)
        //{
        //transform.localScale = new Vector3(-transform.localScale.x, 1, 1f);
        isInCooldown = true;
                //StopAllCoroutines();
                //StartCoroutine(CoolDown(collision));

            //}

        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        speed = 20f;
        Eanimator.SetTrigger("Walking");
        if (rb2.velocity.x <= 0.01)
        {
            ennemyGRX.localScale = new Vector3(-1f, 1, 1f);
        }
        if (rb2.velocity.x >= -0.01)
        {
            ennemyGRX.localScale = new Vector3(1f, 1, 1f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (!isInCooldown)
            {
             //transform.localScale = new Vector3(-transform.localScale.x, 1, 1f);
                isInCooldown = true;
             StartCoroutine(AttackCoolDown(collision));
         
            }

        }
    }
    


    IEnumerator AttackCoolDown(Collision2D collision)
    {
        yield return new WaitForSeconds(attackCooldown);
        isInCooldown = false;
        //Debug.Log("enemy hit " + collision.gameObject.name + Time.time);
        //GetComponent<Enemy>().takeDamage(EnnemyDamage);

    }

    IEnumerator RecalculatingPath(float delay)
    {
        yield return new WaitForSeconds(delay);
        seeker.StartPath(rb2.position, Target.position, OnpathComplete);
        StartCoroutine(RecalculatingPath(delay));
    }
}
