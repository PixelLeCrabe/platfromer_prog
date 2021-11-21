using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider_IA : MonoBehaviour
{
    public float SpiderDamage;
    private float SpiderattakRange;
   
    public float SpiderSpeed;
    private Vector2 move;

    public int SpiderMaxhealth;
    private int Spidercurrenthealth;

    public Transform SpiderAttakPoint;

    public Transform SpiderDetectpoint;
    public float SpiderDetectrange;

    public LayerMask MCLayerMask;

    private Transform playerPos;

    public Animator Sanimator;

    public Rigidbody2D rigidbody2D;


    private void Start()
    {
        playerPos = GameObject.Find("Mc").transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void GettoTarget()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Sanimator.SetTrigger("Walking");
            move.x = SpiderSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, move.x);
        }        
    }
    
    void SpiderAttak ()
    {
        Collider2D[] hitMC = Physics2D.OverlapCircleAll(SpiderAttakPoint.position, SpiderattakRange, MCLayerMask);

        foreach (Collider2D Player in hitMC)
        {
            Debug.Log("enemy hit " + Player.name);

            Player.GetComponent<Enemy>().takeDamage(SpiderDamage);
        }

        Sanimator.SetTrigger("Attaking");

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
        Debug.Log( "Spider" + rigidbody2D.velocity.x);

        Sanimator.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
        GettoTarget();
        SpiderAttak();
        Die();
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = move;
    }
}
