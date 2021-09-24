using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_IA : MonoBehaviour
{
    public float SpiderDamage;
    private float SpiderattakRange;
   
    public float SpiderSpeed;

    public int SpiderMaxhealth;
    private int Spidercurrenthealth;

    public Transform SpiderAttakPoint;

    public Transform SpiderDetectpoint;
    public float SpiderDetectrange;

    public LayerMask MCLayerMask;

    private Transform playerPos;

    public Animator animator;

    public Rigidbody2D rigidbody2D;


    private void Start()
    {
        playerPos = GameObject.Find("Mc").transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void GettoTarget()
    {
        Collider2D[] DetectMC = Physics2D.OverlapCircleAll(SpiderDetectpoint.position, SpiderDetectrange, MCLayerMask); 
        
        foreach (Collider2D Player in DetectMC)
            {
            float move = SpiderSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, move);
        }
        animator.SetTrigger("Walking");
    }
    
    void SpiderAttak ()
    {
        Collider2D[] hitMC = Physics2D.OverlapCircleAll(SpiderAttakPoint.position, SpiderattakRange, MCLayerMask);

        foreach (Collider2D Player in hitMC)
        {
            Debug.Log("enemy hit " + Player.name);

            Player.GetComponent<Enemy>().takeDamage(SpiderDamage);
        }

        animator.SetTrigger("Attaking");
    }

    private void Die()
    {
        if (Spidercurrenthealth < 0)
        {
            Debug.Log("Spider has died");
        }
        // Update is called once per frame        
   
    }
    void Update()
    {
        GettoTarget();
        SpiderAttak();
        Die();
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Player_Speed", Mathf.Abs(rigidbody2D.velocity.x));
    }
}
