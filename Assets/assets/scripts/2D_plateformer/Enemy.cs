using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private EnemyPathfinder enemyPathfinder;
    public Animator animatorR;
    public static Enemy instance;
    private Collider2D collider2D;
   
    public GameObject HPbar;
    
    public float MaxHP;
    public float CurrentHP;
    public float EnnemySpeed;

    public bool isDead;
    public bool hasBeenHit;
   
    void Start()
    {
        hasBeenHit = false;
        HPbar.SetActive(false);
        collider2D = GetComponent<Collider2D>();
        enemyPathfinder = GetComponent<EnemyPathfinder>();
        CurrentHP = MaxHP;
        animatorR = transform.GetChild(0).GetComponent<Animator>();
        instance = this;
    }

    public void takeDamage(float damage)
    {

        if(!isDead)
        {
            hasBeenHit = true;
            CurrentHP -= damage;
            //animatorR.SetBool("IsDamaged", true);
            animatorR.SetTrigger("IsDamaged2");

            // hurt anim

            if (CurrentHP <= 0)
            {
                Die();
            }
        }

    }
    public void hasbeenhit()
    {
        if (hasBeenHit == true)
        {
             HPbar.SetActive(true);
        }
    }
    
    public void Die()
    {
        enemyPathfinder.rb2.velocity = new Vector2(0,0);
        isDead = true;
        animatorR.SetBool("IsDead", true);
        collider2D.enabled = false;
        this.enabled = false;
        enemyPathfinder.enabled = false;       
        //GetComponent<Enemy_HP_bar>().enabled = false;
        //coroutine pour attendre d'active le setactive (false) du l'ennemy
        //enemyPathfinder.gameObject.SetActive(false);

        //desactivate enemy
    }
    private void Update()
    {
        hasbeenhit();
    }
}