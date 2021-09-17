using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controler2D_plateformingv2 : MonoBehaviour 
{
    public Animator animator;
    public event EventHandler OnPpress;

    private enum State
    {
        Normal,
        Rolling,
    }

    private float moveinputX;
    private float moveinputY;
    private float dashinput;
    private Vector2 movedir; 
    private Rigidbody2D Rb2d;
    private float PlayerCurrentSpeed;
    
    private Vector3 RollDir;
    public float RollSpeed;
    private State state;

    private bool isgrounded;
    private bool isdashbuttondown;

    public Transform groundcheck;
    public float checkradius;
    public LayerMask whatisground;
    public float lowjumpmultipliyer;
    public float fallmultipliyer;
    public float dashdistance;
    public float speed;
    public float jumpforce;

    private Vector3 CurrentPostion;
    private float LastMoveDir;

    private int extrajumps;
    public int extrajumpamount;


    private void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        extrajumps = extrajumpamount;
        state = State.Normal;
    }
    void Update()
    {
        switch (state)
        {
            case State.Normal:


                moveinputX = Input.GetAxis("Horizontal");
                movedir = new Vector3(moveinputX, 0);

                PlayerCurrentSpeed = moveinputX * speed;

                // flip the MC
                Vector3 charecterScale = transform.localScale;
                if (Input.GetAxis("Horizontal") < 0)
                {
                    charecterScale.x = -1;
                }
                if (Input.GetAxis("Horizontal") > 0)
                {
                    charecterScale.x = 1;
                }
                transform.localScale = charecterScale;

                //dash direction if player is not moving
                if (moveinputX != 0)
                {
                    LastMoveDir = moveinputX;
                }

                // dashing ground + air
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isdashbuttondown = true;
                }

                // dashing only on the ground
                /*if (Input.GetKeyDown(KeyCode.Space) && isgrounded == true)
                {
                    isdashbuttondown = true;
                } */

                //moveinputY = Input.GetAxis("vertical");

                if (isgrounded == false)
                {
                    animator.SetBool("isJumping", true);
                }

                // switch back to non jumping
                else if (isgrounded == true)
                {
                    animator.SetBool("isJumping", false);
                }

                if (isgrounded == true)
                {
                    extrajumps = extrajumpamount;
                }

                //extra jumps 
               /* if (Input.GetAxis("Vertical")>0.1 && extrajumps > 0)
                {
                    Rb2d.velocity = Vector2.up * jumpforce;
                    extrajumps--;
                }*/

                if (Input.GetKeyDown(KeyCode.UpArrow) && extrajumps > 0)
                {
                    Rb2d.velocity = Vector2.up * jumpforce;
                    extrajumps--;
                }
                //Aled

                if (Input.GetKeyDown(KeyCode.W) && extrajumps > 0)
                {
                    Rb2d.velocity = Vector2.up * jumpforce;
                    extrajumps--;
                }

                else if (Input.GetKeyDown(KeyCode.UpArrow) && extrajumps == 0 && isgrounded == true)
                {
                    Rb2d.velocity = Vector2.up * jumpforce;
                }

                //Jump tweaking (gravitu modifier)
                if (Rb2d.velocity.y < 0)
                {
                    //Rb2d.velocity += Rb2d.velocity * fallmultipliyertest;
                    Rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallmultipliyer - 1) * Time.deltaTime;
                }
                else if (Rb2d.velocity.y > 0 && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowjumpmultipliyer) * Time.deltaTime;
                }

               

                //asing float to the animator
                animator.SetFloat("Player_Speed", Mathf.Abs(moveinputX));

                if (Input.GetKeyDown(KeyCode.M))
                {
                    RollDir = movedir;
                    RollSpeed = 250f;
                        state = State.Rolling;
                }
                break;
 
            case State.Rolling:
                float rollspeeddropmultiplier = 5f;
                RollSpeed -= RollSpeed * rollspeeddropmultiplier * Time.deltaTime;

                float rollspeedminimum = 1.1f;
                if (RollSpeed < rollspeedminimum)
                {
                    state = State.Normal;
                }
                break;

        }
    }
     


    void FixedUpdate()
    {

        switch (state)
        {
      
          case State.Normal:

        isgrounded = Physics2D.OverlapCircle(groundcheck.position, checkradius, whatisground);


                //Dash 
                if (isdashbuttondown)
                {
                    LastMoveDir = LastMoveDir / Mathf.Abs(LastMoveDir);

                    CurrentPostion = new Vector3(transform.position.x + (LastMoveDir * dashdistance), transform.position.y);
                    Rb2d.MovePosition(CurrentPostion);

                    isdashbuttondown = false;
                    CurrentPostion = transform.position;
                }

                 Rb2d.velocity = new Vector2(PlayerCurrentSpeed, Rb2d.velocity.y);
                break;
            case State.Rolling:
                Rb2d.velocity = RollDir * RollSpeed;
               
                break;
        }

        //Rb2d.velocity = Rb2d.velocity = Vector2.up * jumpforce;
        //extrajumps--;
    }  
    
}
