using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHP;
    private float CurrentHP;
    public Animator EnemyAnimator;

    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHP = MaxHP;

    }

    public void takeDamage(float damage)
    {

        if(!isDead)
        {
            CurrentHP -= damage;

            // hurt anim

            if (CurrentHP <= 0)
            {
                Die();
            }
        }

    }

    void Die()
    {
        EnemyAnimator.SetTrigger("isDead");
        isDead = true;

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        //die anim 
        //desactivate enemy
    }

}