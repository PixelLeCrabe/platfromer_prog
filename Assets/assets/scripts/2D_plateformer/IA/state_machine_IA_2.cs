using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_machine_IA_2 : MonoBehaviour
{
    public Transform Spidergrx;
    public Transform Target;

    public Animator Eanimator;
    Rigidbody2D rb2d;

    public int EnnemyDamage;
    public float EnnemySpeed;
    public float Ennemydetectrange;
    public float AttakCD;

    private bool canAttak;
    private bool coolDown;
    private bool targetIsInrange;

    private Vector2 EnnemyMoveDir;
    private Vector2 move;
    private Vector3 TargetPosition;
    
    private State state;
    private enum State
    {
        ChaseTarget,
        idle,
        attaking,
    }


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
            state = State.ChaseTarget;
            //print("target is in range");
        }

    }
    private void Targetoutofrange()
    {
        if (Vector3.Distance(transform.position, TargetPosition) > Ennemydetectrange)
        {
            state = State.idle;
            //print("target our of range");
        }
    }
    void GettoTarget()
    {
        if (Input.GetKey(KeyCode.L))
        {
            //transform.position = Vector3.MoveTowards(transform.position, TargetPosition, move.x);

            //rb2d.velocity = Vector2.right * EnnemySpeed * Time.deltaTime;
            //var positionOffset = (Physics2D.gravity * rb2d.gravityScale) + (new Vector2( Spidergrx.localScale.x, 0) * EnnemySpeed);
            //rb2d.MovePosition(rb2d.position + positionOffset * Time.fixedDeltaTime);
            rb2d.AddForce(new Vector2(1, 0) * EnnemySpeed);
        }
    }

    private void Update()
    {
        FindTargetPosition();
        Flipthecharacter();
        targetinrange();
        Targetoutofrange();
        GettoTarget();
        switch (state)
        {

        }
    }

    private void FixedUpdate()
    {
        GettoTarget();
    }
    IEnumerator CoolDown(Collision2D collision)
    {
        Eanimator.SetTrigger("Attacking");
        yield return new WaitForSeconds(1);
        coolDown = false;
    }
}
