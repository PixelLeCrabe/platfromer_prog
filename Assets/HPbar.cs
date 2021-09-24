using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public Slider slider;
    private int MaxHealt = 100;
    private int currenthealth;
    private int Damage;

    private void Start()
    {
        currenthealth = MaxHealt;
        Damage = 10;
    }

    public void TakeDamage()
    {
        currenthealth = currenthealth - Damage;
        Debug.Log("took" + currenthealth);
    }

   
   
    private void Die()
    {
        if (currenthealth < 0)
        {
            Debug.Log("chracter has died");
        }
    }
    public void SetMaxhealth()
    {
        slider.maxValue = MaxHealt;
        slider.value = currenthealth;

    }

    public void Sethealth()
    {
        slider.value = currenthealth;
    }

    private void Update()
    {
       
        Die();
        
        // for testing 
        if (Input.GetKeyDown(KeyCode.P))
        {
         
           
           //Manage character damage condition etc
            TakeDamage();
           
        }
        //Set Slider Max health value
         SetMaxhealth();

        //Set slider current health Value
         Sethealth();

    }
}