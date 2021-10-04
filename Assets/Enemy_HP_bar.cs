using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_HP_bar : MonoBehaviour
{
    Enemy enemy;

    public Slider Eslider;


    public void SetmaxHP()
    {
        Eslider.maxValue = Enemy.instance.MaxHP;

        Eslider.value = Enemy.instance.CurrentHP;
    }
   
    public void SetHP()
    {
        Eslider.value = Enemy.instance.CurrentHP;
    }

    private void Update()
    {
        SetmaxHP();
        SetHP();
    }
}
