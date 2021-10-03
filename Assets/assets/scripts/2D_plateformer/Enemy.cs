using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private EnemyPathfinder enemyPathfinder;
    public float MaxHP;
    private float CurrentHP;
   // Animator Eanimator;


    public bool isDead;
    void Start()
    {
        enemyPathfinder = GetComponent<EnemyPathfinder>();
        CurrentHP = MaxHP;
       // Eanimator = transform.GetChild(0).GetComponent<Animator>();
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

    public void Die()
    {
        

        isDead = true;
        //Eanimator.SetBool("Isdead", true);
        //GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        enemyPathfinder.enabled = false;
        //coroutine pour attendre d'active le setactive (false) du l'ennemy
        //enemyPathfinder.gameObject.SetActive(false);


        //die anim 
        //desactivate enemy
    }

}