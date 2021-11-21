using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi_state_machine_ia : MonoBehaviour
{
    public Transform Spidergrx;
    public Transform Target;
    public Transform Raycatspoint;

    public Animator Eanimator;
    Rigidbody2D rb2d;

    public float EnnemySpeed;
    public float Ennemydetectrange;

    private bool canAttak;
    private bool coolDown;
    private bool targetIsInrange;

    private Ray2D Obstacledetection;

    private Vector2 EnnemyMoveDir;
    private Vector2 move;
    private Vector3 TargetPosition;
    private enum State
    {
        ChaseTarget,
        idle,
        attaking,
        jumping,
    }

    private State state;

    void Start()
    {
        state = State.idle;
        rb2d = GetComponent<Rigidbody2D>();

    }
    private void Chasetarget()
    {

    }
    private void FindTargetPosition()
    {
        TargetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void Flipthecharacter()
    {
        if (Target.localPosition.x < transform.localPosition.x)
        {
            Spidergrx.localScale = new Vector3(-1f, 1, 1f);
        }
        if (Target.localPosition.x > transform.localPosition.x)
        {
            Spidergrx.localScale = new Vector3(1f, 1, 1f);
        }

        Eanimator.SetTrigger("Walking");
    }
    private void targetinrange()
    {
        if (Vector3.Distance(transform.position, TargetPosition) < Ennemydetectrange)
        {
            print("target is in range");
            
            state = State.ChaseTarget;        
        }
       
    }
    private void Targetoutofrange()
    {
        if (Vector3.Distance(transform.position, TargetPosition) > Ennemydetectrange)
        {
            print("target our of range");
            
            state = State.idle;
        }
    }
    void GettoTarget()
    {             
            //transform.position = Vector3.MoveTowards(transform.position, TargetPosition, move.x);

            //rb2d.velocity = Vector2.right * EnnemySpeed * Time.deltaTime;
            //var positionOffset = (Physics2D.gravity * rb2d.gravityScale) + (new Vector2( Spidergrx.localScale.x, 0) * EnnemySpeed);
            //rb2d.MovePosition(rb2d.position + positionOffset * Time.fixedDeltaTime);
            rb2d.AddForce(new Vector2(Spidergrx.localScale.x, 0) * EnnemySpeed);
        
    }
    private void ObstacleDetected()
    {
        RaycastHit2D hit;
        Debug.DrawRay(new Vector2(Raycatspoint.transform.position.x, Raycatspoint.transform.position.y), new Vector2(1, 0) * 10, Color.red);
        //if (Physics2D.Raycast(new Vector2(Raycatspoint.transform.position.x, Raycatspoint.transform.position.y), new Vector2(1, 0), out hit, 10)
    }
    private void EnnemyJump()
    {

    }


    private void Update()
    {
        Flipthecharacter();
        FindTargetPosition();
        ObstacleDetected();

        switch (state)
        {
            default:
            case State.idle:
                print("state is idle");               
                targetinrange();
            break;

            case State.ChaseTarget:
                print("state is chase target");
                GettoTarget();
                Targetoutofrange();

                break;

            case State.jumping:
                EnnemyJump();
                
            break;
        }
    }

    private void FixedUpdate()
    {
    }
    IEnumerator CoolDown(Collision2D collision)
    {
        Eanimator.SetTrigger("Attacking");
        yield return new WaitForSeconds(1);
        coolDown = false;
    }
}
