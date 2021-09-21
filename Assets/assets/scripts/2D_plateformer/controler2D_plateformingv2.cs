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

    //boutleg
    private bool hasjumped;
    //bool islanded;

    private float moveinputX;
    private float moveinputY;
    private float dashinput;
    private bool IsfallingDown;

    private Vector2 movedir; 
    private Rigidbody2D Rb2d;
    private float PlayerCurrentSpeed;

    private Vector3 RollDir;
    private Vector3 lastRollDir;
    private float airRoll;
    public float airRollamount;

    public float RollSpeed;
    private State state;
    private Vector3 rolldirpos = new Vector3(1, 0, 0);
    private Vector3 rolldirneg = new Vector3(-1, 0, 0);
    private bool isRollinginair;


    public bool isgrounded;
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
    private bool isDoubleJumping;


    private void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
        extrajumps = extrajumpamount;
        state = State.Normal;
        airRoll = airRollamount;



    }

    // update fonctions   

   void DoubleJumpAnimation()
    {
        if (extrajumps == 0 && !isgrounded && !IsfallingDown )
        {
            isDoubleJumping = true; 
        }

        if (isDoubleJumping == true)
        {
            animator.SetTrigger("DoubleJump");
        }
    }

    void MCflip()
    {
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
    }
    void MCjumping()
    {    

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
        }
    
    void Jumpteaking()
    {
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
    }

    void roll()
    {
        // add anti perma dash in the air (there is a float already set up)

        //should add CD on the dash 2 maybe
        if (Input.GetKeyDown(KeyCode.LeftShift) && airRoll >= 0)
        {
            RollDir = movedir;
            state = State.Rolling;
            RollSpeed = 10f;
            airRoll--;


        }
    }

    void fallingdown()
    {
        if (Rb2d.velocity.y < -0.01 && isRollinginair == false)
        {
            IsfallingDown = true;
        }
        else
        {
            IsfallingDown = false;
        }

        if (IsfallingDown == true)
        {
            animator.SetTrigger("FallingDown");
        }
    }

    void rolling()
    {
        animator.SetTrigger("isrolling");

        if (moveinputX == 0)
        {
            RollDir = new Vector3(transform.localScale.x, 0);
        }

        if (moveinputX < 0)
        {
            RollDir = rolldirneg;
        }
        else if (moveinputX > 0)
        {
            RollDir = rolldirpos;
        }


        float rollspeeddropmultiplier = 3;
        RollSpeed -= RollSpeed * rollspeeddropmultiplier * Time.deltaTime;

        float rollspeedminimum = 7f; 
        if (RollSpeed < rollspeedminimum)
        {
            state = State.Normal;
        }

        isRollinginair = true;
        animator.SetBool("MCisRollinginair", true);
    }
   
    // fixed update fonctions 
    void Dash()


    {
        if (isdashbuttondown)
        {
            LastMoveDir = LastMoveDir / Mathf.Abs(LastMoveDir);

            CurrentPostion = new Vector3(transform.position.x + (LastMoveDir * dashdistance), transform.position.y);
            Rb2d.MovePosition(CurrentPostion);

            isdashbuttondown = false;
            CurrentPostion = transform.position;
        }

        Rb2d.velocity = new Vector2(PlayerCurrentSpeed, Rb2d.velocity.y);
    }

    void Mchasjumpedonce()
    {
        if (Rb2d.velocity.y > .01)
        {
            hasjumped = true;
        }
    }

    // Test zone start

    /*void landedanimtest()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<Animator>().Play("MC_landed");
        }
    }
    */
   
    // test Zone end 
   
    //fix that shit its alway true gotta be trigger only when it lands
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Sol" && hasjumped == true)
        {
            
            //animator.SetBool("islanded",true);
            GetComponent<Animator>().Play("MC_landed");

        }
    }

    void Update()
    {
        switch (state)
        {
            case State.Normal:

                // Test zone start

               

                //test zone end

                //mega boutleg cheking if the player has jumped one               
                Mchasjumpedonce();

                moveinputX = Input.GetAxis("Horizontal");
                movedir = new Vector3(moveinputX, 0);

                PlayerCurrentSpeed = moveinputX * speed;

                // flip the MC
                MCflip();

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

                if (isgrounded == false && IsfallingDown == false && !isDoubleJumping && isRollinginair == false)
                {
                    animator.SetBool("isGrounded", false);
                    animator.SetBool("isJumping", true);
                }
              

                // switch back to non jumping
                if ((isgrounded == false && IsfallingDown == true) || isDoubleJumping)
                {
                    animator.SetBool("isGrounded", false);
                    animator.SetBool("isJumping", false);
                }
                

                if (isgrounded == true)
                {
                    airRoll = airRollamount;
                    extrajumps = extrajumpamount;
                    animator.SetBool("isGrounded", true);
                    isDoubleJumping = false;
                    animator.SetBool("DoubleJump", false);
                    isRollinginair = false;
                    animator.SetBool("MCisRollinginair", false);
                    animator.SetBool("isJumping", false);
                }

                //extra jumps + MC jump
                
                MCjumping();

                // tweak du jump via gravity multipliyer UwU
                Jumpteaking();



                DoubleJumpAnimation();

                //asing float to the animator
                animator.SetFloat("Player_Speed", Mathf.Abs(moveinputX));

                roll();


                fallingdown();

                //MCLanded();

                break;
 
            case State.Rolling:
                //
                rolling();
         //
                break;

                
        }
    }
     


    void FixedUpdate()
    {

        switch (state)
        {
            //if(circleColider.isTrigger && circleColider.e )
            //isgrounded = circleColider.isTrigger
            case State.Normal:
                isgrounded = Physics2D.OverlapCircle(groundcheck.position, checkradius, whatisground);


                //Dash 
                Dash();
                break;
            case State.Rolling:
                Rb2d.velocity = RollDir * RollSpeed;
               
                break;
        }

        //Rb2d.velocity = Rb2d.velocity = Vector2.up * jumpforce;
        //extrajumps--;
    }  
    
}
