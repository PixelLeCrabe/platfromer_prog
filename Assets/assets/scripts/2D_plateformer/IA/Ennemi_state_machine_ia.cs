using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemi_state_machine_ia : MonoBehaviour
{
    [Header("transforms")]
    public Transform Spidergrx;
    public Transform Target;
    public Transform RaycatsPoint;
    [SerializeField] Transform Ennemygroundcheck;

    public LayerMask ObstacleLayer;

    Animator Eanimator;
    Rigidbody2D rb2d;
    Collider2D Col2d;

    private  controller_test Controller;
    private thrown_item trhownItem;
    private Enemy enemy;

    [Header("Enemy Parameters")]
    [SerializeField] float EnnemySpeed; // correct amount is 20
    [SerializeField] float JumpForce;
    public float EnnemyCurrentSpeed;
    public float Ennemydetectrange;
    public float Ennemyfallmultipliyer;
    public float baseAttakKnockbackAmount;

    private bool targetIsInrange;
    private bool iscollidingwithMC;
    private bool Ennemyisgrounded;
    private bool ObstacleInFront;

    private Ray2D Obstacledetection;

    private Vector2 Raycastpoint;

    private Vector3 TargetPosition;
    private enum State
    {
        ChaseTarget,
        idle,
        attaking,
        jumping,
        Dead,
        Hit,
        Grabed,
        LandafterThrow,
    }

    private State state;

    void Start()
    {

        Eanimator = GetComponentInChildren<Animator>();
        state = State.idle;
        rb2d = GetComponent<Rigidbody2D>();
        EnnemyCurrentSpeed = EnnemySpeed;
        enemy = GetComponent<Enemy>();
        trhownItem = GetComponent<thrown_item>();
    }
    private void FindTargetPosition()
    {
        TargetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void FlipTheCharacter()
    {
        if (Target.localPosition.x < transform.localPosition.x)
        {
            Spidergrx.localScale = new Vector3(-1f, 1, 1f);
        }
        if (Target.localPosition.x > transform.localPosition.x)
        {
            Spidergrx.localScale = new Vector3(1f, 1, 1f);
        }
    }
    private void targetinrange()
    {
        if (Vector3.Distance(transform.position, TargetPosition) < Ennemydetectrange && !transform.GetComponent<Enemy>().isDead && !transform.GetComponent<Enemy>().isHit && !ObstacleInFront)
        {
            //print("target is in range");
            
            state = State.ChaseTarget;        
        }
    }
    private void TargetOutofRange()
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
        
        // this is gettin him crazy when thrown in the air
        if (!Ennemyisgrounded) return;
        //print("Gettingtotarget");
        rb2d.AddForce(new Vector2(Spidergrx.localScale.x, 0) * EnnemyCurrentSpeed * Time.deltaTime, ForceMode2D.Impulse);            
    }
    
    void IsGrabed()
    {
        if(trhownItem.ItemIsGrabed)
        {       
            state = State.Grabed;
        }
    }
    void ObstacleDetected()
    {
        Debug.DrawRay(new Vector2(Raycastpoint.x, Raycastpoint.y), new Vector2 (Spidergrx.localScale.x, 0 )*.2f, Color.red);
        
        RaycastHit2D hit = Physics2D.Raycast(Raycastpoint, new Vector2 (Spidergrx.localScale.x ,0) , .3f, ObstacleLayer);
        if (hit)
        {
            ObstacleInFront = true;
            //print("there is an obstacle at 0.2");
            state = State.jumping;
        }
        else 
            ObstacleInFront = false;
    }
    private void EnnemyJump()
    {
        //print("Spider supposed to jump");
        rb2d.AddForce(new Vector2(Spidergrx.localScale.x * .3f, 1 ) * JumpForce, ForceMode2D.Impulse);

    }
    private void IsDead()
    {
        if (transform.GetComponent<Enemy>().isDead)
        {
            print("Ennemy is Dead");
            StartCoroutine(CoroutineWaitFordeath());
        }
    }
    private void CollidingwithMC()
    {
        if (iscollidingwithMC == true)
            EnnemyCurrentSpeed = 0;
        else
            EnnemyCurrentSpeed = EnnemySpeed;
    } 
    private void HitStagger()
    {        
        Vector2 difference = transform.position - TargetPosition;
        difference = difference.normalized;
        rb2d.AddForce(difference * baseAttakKnockbackAmount, ForceMode2D.Impulse);
        
        //rb2d.AddForce(new Vector2(Spidergrx.localScale.x, 0) * EnnemyCurrentSpeed, ForceMode2D.Force);
        //rb2d.AddForce(new Vector2 (((Math.Abs(transform.position.x) + baseAttakKnockbackAmount) * Spidergrx.localScale.x), 0), ForceMode2D.Impulse);

        //print("stagger force applied");
    }
    private void isHit()
    {
        if (transform.GetComponent<Enemy>().isHit)
        {
            print("Ennemy is hit");
            state = State.Hit;
        }
    }
  
    //Spider is colliding with MC
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            iscollidingwithMC = true;
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            iscollidingwithMC = false;
    }
    
    private void Update()
    {
        //Groundcheck
        // Need to be teawked !         /!\
        // ground check transform position have to be y = -0.122
        Ennemyisgrounded = Physics2D.OverlapCircle(Ennemygroundcheck.position, .01f, ObstacleLayer);

        //obstacle check
        Raycastpoint = new Vector2(RaycatsPoint.transform.position.x, RaycatsPoint.transform.position.y);
        
        //Lui la c'est un fdp ya plein de boolean à gerer
        targetinrange();

        FlipTheCharacter();
        FindTargetPosition();
        CollidingwithMC();
        IsGrabed();

        switch (state)
        {
            default:
            case State.idle:
                //print("state is idle");
                IsDead();
                break;

            case State.ChaseTarget:
                //print("state is chase target");
                GettoTarget();
                TargetOutofRange();
                IsDead();
                isHit();
                ObstacleDetected();
                break;

            case State.jumping:
                //print("state is jumping");

                ObstacleDetected();
                EnnemyJump();
                if (!ObstacleInFront)
                    state = State.ChaseTarget;
                break;

            case State.Hit:
                HitStagger();
                IsDead();
                print("State is Hit");
                break;

            case State.Dead:
                print("State is dead");
                this.enabled = false;
                break;

            case State.Grabed:
                IsDead();

                Eanimator.SetBool("Grabbed", true);
                print(" Enemy State is grabed");

                //Getting out of the state
                if(Grab.instance.IsHoldingAgrabedItem == false)
                {
                    //Eanimator.Play("spieder_grabbed");
                    state = State.idle;
                    print("Get out of Grabed state");
                }                
                break;

            case State.LandafterThrow:

                break;
        }
    }
    #region coroutines
    IEnumerator CoroutineWaitFordeath()
    {
        yield return new WaitForSeconds(.2f);
        state = State.Dead;
    }
    IEnumerator CoroutineDamagedDuration()
    {
        yield return new WaitForSeconds(.2f);
        print("is not damaged anymore");
    }
    #endregion
}
