using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_HP_bar : MonoBehaviour
{
    public Slider Eslider;


    public void SetmaxHP()
    {
        Eslider.maxValue = transform.GetComponent<Enemy>().MaxHP;

        Eslider.value = transform.GetComponent<Enemy>().CurrentHP;
    }
   
    public void SetHP()
    {
        Eslider.value = transform.GetComponent<Enemy>().CurrentHP;
    }

    private void Update()
    {
        SetmaxHP();
        SetHP();
    }
}
