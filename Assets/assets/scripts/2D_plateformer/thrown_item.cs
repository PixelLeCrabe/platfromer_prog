using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrown_item : MonoBehaviour
{
    #region Variables
    public MediumProps mediumprops;
    
    private Grab grab;

    public Transform DmgPoint;

    [SerializeField] LayerMask DamagableEntities;

    [HideInInspector]public bool ItemIsGrabed;

    private Collider2D DMGradius;
    #endregion
    private void Start()
    {
        DMGradius = GetComponent<Collider2D>();
        
        // transform can be unique to differents entities
        //DmgPoint = GetComponentInChildren<Transform>();
    }

    void ExplosionZone()
    {
        /* Gotta make this work to have a zone dmg
        Collider2D[] hitEntity = Physics2D.OverlapCircleAll(DmgPoint.position, mediumprops.DMGRange, DamagableEntities);
        foreach (Collider2D enemy in hitEntity)
        {
           
            if(enemy.GetComponent<Enemy>() != null)
            {
                // DMG the entites
                enemy.GetComponent<Enemy>().takeDamage(mediumprops.DMGAmount);
            }

            // Camera Shake amount
        }
            Cinemachine_cameraShake.instance.MCAttackShake(mediumprops.CameraShakeAmount, 0.2f);
         */     
       
    }
    private void OnCollisionEnter2D(Collision2D C)
    {
        if(Grab.instance.GrabedItemName == gameObject.name && Grab.instance.ItemIsThrown)
        {
            print("has touched " + C.gameObject.layer);
        }
    }
    void OnTriggerEnter2D(Collider2D C)
    {
        /* Filter entities that can be ,hit by the projectile
        if ((C.gameObject.layer == LayerMask.NameToLayer("enemy") || C.gameObject.layer == LayerMask.NameToLayer("sol")) && Grab.instance.GrabedItemName == gameObject.name && Grab.instance.ItemIsThrown) 
        {
            // Reset bool for obj thrown
            Grab.instance.ItemIsThrown = false;

            ExplosionZone();
            print("Barrel touched : " + C.gameObject.name);

            // Can be tweeked by changin the collider size maybe  /KEEP THAT\
            //C.GetComponent<Enemy>().takeDamage(mediumprops.DMGAmount);

            
            // dind't work it called the function 10 times instant killing the enemys
            /*
            // Run through the Entities and Damage thems
            foreach (Collider2D enemy in hitEntity)
                {
                    // Camera Shake amount
                    Cinemachine_cameraShake.instance.MCAttackShake(mediumprops.CameraShakeAmount, 0.2f);
                    

                    print(C.gameObject.name + " Is damaged by a barrel");
                }
              */
        }
}
