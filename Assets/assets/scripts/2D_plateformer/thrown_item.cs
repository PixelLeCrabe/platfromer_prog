using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrown_item : MonoBehaviour
{
    #region Variables
    public MediumProps mediumprops;
    
    private Grab grab;

    private Transform DmgPoint;

    [SerializeField] LayerMask DamagableEntities;

    private bool HasBeenGrabOnce;
    public bool ItemIsGrabed;
    #endregion
    private void Start()
    {
        // transform can be unique to differents entities
        DmgPoint = GetComponentInChildren<Transform>();    
    }
    
    void OnTriggerEnter2D(Collider2D C)
    {
        // Filter entities that can be ,hit by the projectile
        if (C.gameObject.layer == LayerMask.NameToLayer("enemy") && Grab.instance.GrabedItemName == gameObject.name) // No hit when the barrel is realesed
        { 
            print("Barrel touched : " + C.gameObject.name);
            
            Collider2D[] hitEntity = Physics2D.OverlapCircleAll(DmgPoint.position, mediumprops.DMGRange, DamagableEntities);

            foreach (Collider2D enemy in hitEntity)
                {
                    Cinemachine_cameraShake.instance.MCAttackShake(.5f, 0.2f);
                    enemy.GetComponent<Enemy>().takeDamage(mediumprops.DMGAmount);

                    print(gameObject.name + " Is damaged by a barrel");
                }
            }
            //if (!HasBeenGrabOnce) return;

            //Collider2D[] hitEntity = Physics2D.OverlapCircleAll(DmgPoint.position, mediumprops.DMGRange, DamagableEntities);

            /*foreach (Collider2D enemy in hitEntity)
            {
                //Cinemachine_cameraShake.instance.MCAttackShake(.5f, 0.2f);
                //enemy.GetComponent<Enemy>().takeDamage(DmgAmount);

                print(enemy.tag + " Is damaged by a barrel");
            }
            Debug.Log(gameObject.name + " Hit the ground");
            */
            // Not relevent for now
            //gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;        
        }
        void Update()
    {    }
}
