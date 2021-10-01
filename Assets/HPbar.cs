using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public static HPbar instance;
    public Slider slider;
    private int MaxHealt = 100;
    public float currenthealth;
    public bool isinvisible = false;
    private float Damage = 10f;

    private void Start()
    {
        currenthealth = MaxHealt;
        
    }

    private void Awake()
    {
        instance = this;
    }

    public void TakeDamage()
    {
        if (!isinvisible)
        {
            currenthealth = currenthealth - Damage;
            Debug.Log("took" + currenthealth);

        }
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
        //Set Slider Max health value
         SetMaxhealth();

        //Set slider current health Value
         Sethealth();

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage();
        }
    }
}