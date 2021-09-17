using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaC_combat_systeme : MonoBehaviour
{
    public Animator animator;

    public float MCAttackDamage; 
    public Transform MCattackpoint;
    public float MCattackRange = 0.5f;
    public LayerMask EnemyLayerMask;
    
    public float MCattackRate = 2f;
    float nextAttack = 0f;
   
    void Update()
    {
        if(Time.time >= nextAttack)
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

           Collider2D[] hitEnnemy = Physics2D.OverlapCircleAll(MCattackpoint.position, MCattackRange,EnemyLayerMask) ;
        
            foreach(Collider2D enemy in hitEnnemy)
            {
                Debug.Log("enemy hit " + enemy.name);

                enemy.GetComponent<Enemy>().takeDamage(MCAttackDamage);
            }
        }
       
    }
    void OnDrawGizmoSelected()
    {
        Gizmos.DrawWireSphere(MCattackpoint.position, MCattackRange);
    }
}



