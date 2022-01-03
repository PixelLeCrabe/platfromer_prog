using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaC_combat_systeme : MonoBehaviour
{
    public static CaC_combat_systeme instance;

    public Animator animator;

    public Transform MCattackpoint;
    public Transform MCSpecialAttakpoint;

    private controller_test Controller_Test;

    [SerializeField] private float MCattackRange = 0.2f;
    [SerializeField] private float MCSpecialAttakrange = 0.5f;
    [SerializeField] private float BaseAttackDelay;
    [SerializeField] private float SpecialAttackDelay;
    public float MCAttackDamage;
    public float MCattackRate = 2f;
    public float MCSpecialAttakDammage;
    public float Specialattakrate =0.2f;
    private float nextAttack;
    private float nextSpecialattak;

    public bool isAttacking;
    public bool isSpecialAttacking;

    public const string PLAYER_ATTACKING = ("King_attak_bas_part1");
    public const string PLAYER_SPECIAL_ATTACKING = ("");

    public LayerMask EnemyLayerMask;

    private void Awake()
    {
        instance = this;
        Controller_Test = GetComponent<controller_test>();
    }

    void Update()
    {
        if (Time.time >= nextAttack)
        {
            if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire2")) 
            {
                isAttacking = true;
                Attack();
                // Animation
                if (isAttacking)
                {
                    Controller_Test.PlayerAnimationState(PLAYER_ATTACKING);
                }
                nextAttack = Time.time + 1f / MCattackRate;
                StartCoroutine(CoroutineBaseAttackDelay());
            }
        }
          void Attack()
        {                      
            Collider2D[] hitEnnemy = Physics2D.OverlapCircleAll(MCattackpoint.position, MCattackRange, EnemyLayerMask);
            foreach (Collider2D enemy in hitEnnemy)
            {
                //camera shake         
                Cinemachine_cameraShake.instance.MCAttackShake(.5f, 0.2f);
                enemy.GetComponent<Enemy>().takeDamage(MCAttackDamage);
            }                      
        }

        if (Time.time > nextSpecialattak)      
        { 
            if (Input.GetKeyDown(KeyCode.Y) ) //|| Input.GetButtonDown("SpecialAttack"))
            {
                isSpecialAttacking = true;
                SpecialAttak();
                if (isSpecialAttacking)
                {
                    Controller_Test.PlayerAnimationState(PLAYER_SPECIAL_ATTACKING);
                }
                nextSpecialattak = Time.time + 1F / Specialattakrate;
                StartCoroutine(CoroutineSpecialAttackDelay());
            }
        }

        void SpecialAttak()
        {
            Collider2D[] hitEnnemy = Physics2D.OverlapCircleAll(MCSpecialAttakpoint.position, MCattackRange, EnemyLayerMask);

            foreach (Collider2D enemy in hitEnnemy)
            {
                Debug.Log("enemy Special hit " + enemy.name);
                Cinemachine_cameraShake.instance.MCSpecialAttackShake(1f, 0.2f);
                enemy.GetComponent<Enemy>().takeDamage(MCSpecialAttakDammage);
            }                                   
        }
    }

    IEnumerator CoroutineBaseAttackDelay()
    {
        yield return new WaitForSeconds(BaseAttackDelay);
        isAttacking = false;
    }
    IEnumerator CoroutineSpecialAttackDelay()
    {
        yield return new WaitForSeconds(SpecialAttackDelay);
        isSpecialAttacking = false;
    }
}



