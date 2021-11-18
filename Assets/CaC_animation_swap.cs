using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaC_animation_swap : MonoBehaviour
{
    public Animator animator;
   
    private bool CanSecondAttak;
    private bool CanAttakk;
    private bool firstBaseAttak;
    private float Attakinput;

    public float SecondAttaktimer;
    public float baseattakCD;

    private int nbofAttakIpunt;
    
    void Start()
    {
        nbofAttakIpunt = 0;
        //CanSecondAttak = true;
        CanAttakk = true;
        //CanSecondAttak = true;
        animator.SetBool("CantfirstAttak", false);
    }

    private void BaseAttak()
    {
        if (CanAttakk)
        {
            if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire2")) && !CanSecondAttak)
            {
                //base Attak CD
                StartCoroutine(coroutineBaseAttakCD());
                //retun to base attack if there is only on input after a certain aùpount of time 
                //StartCoroutine(coroutineSecondAttakTimer());
                animator.SetTrigger("baseAttack1");
                Debug.Log("base attak 1");
                //Wait for 2nd Attak
                StartCoroutine(coroutineCanSecondAttak());
                StartCoroutine(coroutinenbofinputreset());
                CanAttakk = false;
            }

            if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire2")) && CanSecondAttak == true)
            {
                print("Second attak");
                CanSecondAttak = false;
                CanAttakk = false;
                
                //Manimator.SetBool("CantfirstAttak", false);
                animator.SetTrigger("baseAttack2");
                //StartCoroutine(coroutineCanSecondAttak());
                //base Attak CD
                StartCoroutine(coroutineBaseAttakCD());
                //reset number of attack input
                StartCoroutine(coroutinenbofinputreset());
            }
        }
    }
    
    private void Attackinputcount()
    {
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.T))// && CanAttakk)
        {
            nbofAttakIpunt++;
            
            //test reset nb of input whil in Attack CD
            StartCoroutine(coroutinenbofinputresetInCD());
        }
    }

    private void canSecondAttakCheck()
    {
        if (nbofAttakIpunt > 1)
        {
            CanSecondAttak = true;
        }
    }
    void Update()
    {
        Attackinputcount();
        canSecondAttakCheck();
        BaseAttak();
        if (Input.GetKeyDown(KeyCode.T))
            {
            print("Tpressed");
            }

        if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire2")) && CanSecondAttak)
            print("cansecondattack is true");
           
    }
    IEnumerator coroutineSecondAttakTimer()
    {
        yield return new WaitForSeconds(SecondAttaktimer);
        CanSecondAttak = false;

    }

    IEnumerator coroutineBaseAttakCD()
    {
        yield return new WaitForSeconds(baseattakCD);
        CanAttakk = true;
    }

    IEnumerator coroutineCanSecondAttak()
    {
        yield return new WaitForSeconds(.2f);
        animator.SetBool("CantfirstAttak", false);
    }

    IEnumerator coroutinenbofinputreset()
    {
        yield return new WaitForSeconds(.5f);
        nbofAttakIpunt = 0;
        //test
        CanSecondAttak = false;
    }

    IEnumerator coroutinenbofinputresetInCD()
    {
        yield return new WaitForSeconds(.5f);
        nbofAttakIpunt = 0;
    }
}
