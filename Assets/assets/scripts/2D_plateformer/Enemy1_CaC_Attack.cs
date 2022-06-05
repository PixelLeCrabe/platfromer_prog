using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1_CaC_Attack : MonoBehaviour
{
    private bool isInCooldown;
    public bool CanAttak;
    public float EnemyAttack;
    public float AttackSpeed = 2f;
    public float EnemyAttackCD;
    public bool AbletoAttack;

    public Transform EnemyAttackpoint;

    public LayerMask targetLayerMask;
    public float EnemyAttackRange;
    public float EnemyAttackDamage;
    public Animator Eanimator;

    private void Start()
    {
        CanAttak = true;
    }

    void Update()
    {
        if (CanAttak && AbletoAttack)
        {
            StartCoroutine(CoroutineAttackCoolDown());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AbletoAttack = true;
        }
    }

    private void EnmyAttak()
    {
        if (AbletoAttack == true)
        {
           // EnemyNextAttack = Time.time + 1f / EnemyAttack
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        AbletoAttack = false;
    }
    IEnumerator CoroutineAttackCoolDown()
    {
        CanAttak = false;
        Eanimator.SetBool("Attacking", true);
        StartCoroutine(CoroutineAnimationAttack());
        HPbar.instance.TakeDamage();
        //print("has attack");
        yield return new WaitForSeconds(EnemyAttackCD);
        CanAttak = true;
    }


    // Be careful to twick the coroutione value depending on the animation
    IEnumerator CoroutineAnimationAttack()
    {
        yield return new WaitForSeconds(0.5f);
        Eanimator.SetBool("Attacking", false);
    }
}
