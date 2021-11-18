using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ennemy_IA : MonoBehaviour
{
    Rigidbody2D rb2d;

    public LayerMask TargetLayerMask;

    public Animator Eanimator;

    // pathinding
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachendofPath = false;

    Seeker seeker;

    //public Transform  MCposition;
    public Vector3 MCposition;
    public Transform Spidergrx;
    public Transform Target;
    private Vector3 DistanceToMC;

    private Vector3 EnnemyPosition;

    public int EnnemyDamage;
    public float EnnemyAttakrange;
    public float Ennemydetectrange;
    public Transform EnnemyAttakrangePoint;

    public float AttakCD;
    public bool canAttak;

    private float NextAttack;
    public float EAttackRate;
    public bool coolDown;
    public float EnnemySpeed;

    private enum State
    {
        
        ChaseTarget,
        idle,
        attaking,
    }

    private State state;

    void Start()
    {
        state = State.idle;
        rb2d = GetComponent<Rigidbody2D>();

        seeker = GetComponent<Seeker>();

        seeker.StartPath(rb2d.position, Target.position, OnpathComplete);

    }

    void OnpathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FindMcPosition()
    {
        MCposition = GameObject.FindGameObjectWithTag("Player").transform.position;       
    }

    private void idle()
    {
        Eanimator.SetTrigger("Idle");
        //GetComponent<Animator>().Play("spider_idle");
    }

    private void targetinrange()
    {
        if (Vector3.Distance(transform.position, MCposition) < Ennemydetectrange)
        {
            state = State.ChaseTarget;
        }

             //DistanceToMC = (Transform.position);
    }

    private void Findthetarget()
    {

        // Flip the Spider
        if (Target.localPosition.x < transform.localPosition.x)
        {
            Spidergrx.localScale = new Vector3(-1f, 1, 1f);
        }
        if (Target.localPosition.x > transform.localPosition.x)
        {
            Spidergrx.localScale = new Vector3(1f, 1, 1f);
        }

        Eanimator.SetTrigger("Walking");
       /* 
        // the spider copy thes scale of the MC making it face backward when the Mc run at it
        Vector3 dirction = transform.position - MCposition;

        // Normalize resultant vector to unit Vector.
        dirction = -dirction.normalized;
        dirction.y = 0;

        // Move in the direction of the direction vector every frame.
        transform.position += dirction * Time.deltaTime * EnnemySpeed;
       */
    }

    //Not used cuz buged
    private void Targetoutofrange()
    {
        if (Vector3.Distance(transform.position, MCposition) < Ennemydetectrange + 1f)
        {
            state = State.idle;
        }
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

    // Update is called once per frame
    void Update()
    {
       
        Eanimator.SetFloat("speed", rb2d.velocity.x);

        if (!coolDown)
        {
            FindMcPosition();
        }

        switch (state)
        {
            default:  
            case State.idle:
                //Debug.Log("state is Chase idle");
               
                //FindMcPosition();
                targetinrange();

            break;

            case State.ChaseTarget:
                Debug.Log("state is Chase target");
                Findthetarget();
                //Targetoutofrange();

           break;
            
        }
       
      

    }

     void FixedUpdate()
    {
        switch (state)
        {
         case State.ChaseTarget:
                Debug.Log("state is Chase target");
      

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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;

                Vector2 force = direction * EnnemySpeed * Time.deltaTime;

                rb2d.AddForce(force);
                
                float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance) 
                {
                    currentWaypoint++;
                }


           break;
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
