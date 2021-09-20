using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaC_combat_systeme : MonoBehaviour
{
    public Animator animator;

    public float MCAttackDamage;
    public float MCattackRange = 0.2f;
    public Transform MCattackpoint;
    public float MCattackRate = 2f;
     private float nextAttack = 0f;
    
    public float MCSpecialAttakDammage;
    public float MCSpecialAttakrange = 0.5f;
    public Transform MCSpecialAttakpoint;
    public float Specialattakrate =0.2f;
    private float nextSpecialattak = 0f;

    public LayerMask EnemyLayerMask;


    void Update()
    {
        if (Time.time >= nextAttack)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Attack();
                nextAttack = Time.time + 1f / MCattackRate;
            }
        }
        void Attack()
        {
            animator.SetTrigger("Attack");

            Collider2D[] hitEnnemy = Physics2D.OverlapCircleAll(MCattackpoint.position, MCattackRange, EnemyLayerMask);

            foreach (Collider2D enemy in hitEnnemy)
            {
                Debug.Log("enemy hit " + enemy.name);

                enemy.GetComponent<Enemy>().takeDamage(MCAttackDamage);
            }
        }

        if (Time.time > nextSpecialattak)      
        { 
            if (Input.GetKeyDown(KeyCode.Y))
            {
                SpecialAttak();
                nextSpecialattak = Time.time + 1F / Specialattakrate;
                Debug.Log("Special atttatak");
               }
        }

        void SpecialAttak()
        {
            animator.SetTrigger("SpecialAttak");
            Collider2D[] hitEnnemy = Physics2D.OverlapCircleAll(MCSpecialAttakpoint.position, MCattackRange, EnemyLayerMask);

            foreach (Collider2D enemy in hitEnnemy)
            {
                Debug.Log("enemy Special hit " + enemy.name);

                enemy.GetComponent<Enemy>().takeDamage(MCSpecialAttakDammage);

            }
            void OnDrawGizmoSelected()
            {
                Gizmos.DrawWireSphere(MCattackpoint.position, MCattackRange);
            }
        }
    }
}


