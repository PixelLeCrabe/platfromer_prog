using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemy_IA : MonoBehaviour
{
    public LayerMask TargetLayerMask;

    private Vector3 DistanceToMC;

    public Animator Eanimator;

    //public Transform  MCposition;
    public Vector3 MCposition;
    public Transform MCpositioN;

    private Vector3 EnnemyPosition;

    public int EnnemyDamage;
    public float EnnemyAttakrange;
    public float Ennemydetectrange;
    public Transform EnnemyAttakrangePoint;

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
        
    }

    private void FindMcPosition()
    {
        MCposition = GameObject.FindGameObjectWithTag("Player").transform.position;
        
    }

    private void idle()
    {
        GetComponent<Animator>().Play("spider_idle");
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
        Vector3 dirction = transform.position - MCposition;

        // Normalize resultant vector to unit Vector.
        dirction = -dirction.normalized;

        // Move in the direction of the direction vector every frame.
        transform.position += dirction * Time.deltaTime * EnnemySpeed;

        if (Vector3.Distance(transform.position, MCposition) < EnnemyAttakrange)
        {
            state = State.attaking;
        }
    }

    private void Targetoutofrange()
    {
        if (Vector3.Distance(transform.position, MCposition) < Ennemydetectrange + 1f)
        {
            state = State.idle;
        }
    }
   
    private void Attak()
    {
        GetComponent<Animator>().Play("spider_attak");

        Collider2D[] hitMC = Physics2D.OverlapCircleAll(EnnemyAttakrangePoint.position, EnnemyAttakrange, TargetLayerMask);

        foreach (Collider2D Player in hitMC)
        {
            Debug.Log("enemy hit " + Player.name);

            Player.GetComponent<Enemy>().takeDamage(EnnemyDamage);
        }

    }

    // Update is called once per frame
    void Update()
    {
        FindMcPosition();
        switch (state)
        {
            default:  
            case State.idle:
                Debug.Log("state is Chase idle");

                FindMcPosition();

                targetinrange();

            break;

            case State.ChaseTarget:
                Debug.Log("state is Chase target");
                Findthetarget();
                Targetoutofrange();


           break;

            case State.attaking:
                
                Debug.Log("state is Chase attaking");


                Attak();
       
                break;
        }

    }
}
