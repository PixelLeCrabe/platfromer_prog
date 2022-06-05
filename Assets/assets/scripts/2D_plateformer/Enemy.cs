using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animatorR;
   
    private Collider2D collider2D;
   
    public GameObject HPbar;

    [SerializeField] public float Staggerduration;
    public float MaxHP;
    public float CurrentHP;

    public bool isHit;
    public bool isDead;
    public bool hasBeenHit;
   
    void Start()
    {
        hasBeenHit = false;
        HPbar.SetActive(false);
        collider2D = GetComponent<Collider2D>();
        CurrentHP = MaxHP;
        animatorR = transform.GetChild(0).GetComponent<Animator>();
    }

    public void takeDamage(float damage)
    {
        if(!isDead)
        {
            hasBeenHit = true;
            CurrentHP -= damage;
            isHit = true;
            StartCoroutine(StaggerDuration());
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
        isDead = true;
        animatorR.SetBool("IsDead", true);
        collider2D.enabled = true;
        this.enabled = false;
        //enemyPathfinder.enabled = false;       
        //GetComponent<Enemy_HP_bar>().enabled = false;
        //coroutine pour attendre d'active le setactive (false) du l'ennemy
        //enemyPathfinder.gameObject.SetActive(false);

        //desactivate enemy
    }
    private void Update()
    {
        hasbeenhit();
    }
    IEnumerator StaggerDuration()
    {
        yield return new WaitForSeconds(Staggerduration);
        isHit = false;
    }
}
