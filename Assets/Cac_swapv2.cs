using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cac_swapv2 : MonoBehaviour
{
    public Animator animator;

    private bool CanSecondAttak;
    private bool CanAttakk;
    private bool firstBaseAttak;
    private float Attakinput;

    public float baseattakCD;

    void Start()
    {
        //CanSecondAttak = true;
        CanAttakk = true;
        CanSecondAttak = false;
        //CanSecondAttak = true;
    }

    private void BaseAttak()
    {
        if (CanAttakk)
        {
            if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire2")) && !CanSecondAttak)
            {
                CanAttakk = false;
                //CanSecondAttak = true;
                animator.SetTrigger("baseAttack1");
                Debug.Log("base attak 1");
                StartCoroutine(coroutineCanSecondAttak());
                StartCoroutine(coroutineCanTSecondAttak());
                if ( CanSecondAttak)
                    {
                        
                    }

                if ((Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("Fire2")) && CanSecondAttak)
                {
                    CanAttakk = false;
                    print("Second attak");
                    animator.SetTrigger("baseAttack2");
                    StartCoroutine(coroutineBaseAttakCD());
                }
            }
        }
    }

    void Update()
    {
        BaseAttak();
    }

    IEnumerator coroutineBaseAttakCD()
    {
        yield return new WaitForSeconds(baseattakCD);
        CanAttakk = true;
        CanSecondAttak = false;
    }

    IEnumerator coroutineCanSecondAttak()
    {
        yield return new WaitForSeconds(.2f);
        animator.SetBool("CantfirstAttak", false);
        CanSecondAttak = true;
    }

    IEnumerator coroutineCanTSecondAttak()
    {
        yield return new WaitForSeconds(.3f);
        CanSecondAttak = false;
    }
}

