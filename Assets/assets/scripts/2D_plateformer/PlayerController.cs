using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rb2D;

    [SerializeField] LayerMask whatisground;
   
    [SerializeField] Transform groundcheck;

    [SerializeField] Transform Player;

    private State state;
    
    private Vector3 RollDir;
    private Vector3 lastRollDir;
    private Vector3 PlayerWorldPosition;
    private Vector2 movedir;
    //private Vector3 rolldirneg = new Vector3(-1, 0, 0);
    //private Vector3 rolldirpos = new Vector3(1, 0, 0);
    //private Vector3 CurrentPostion;

    private bool isgrounded;

    [SerializeField] private int extrajumpamount;
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float lowjumpmultipliyer;
    [SerializeField] private float fallmultipliyer;
    [SerializeField] private float groundcheckradius;
    private int extrajumps;
    private float moveinputX;
    private float PlayerCurrentSpeed;


    void Start()
    {
        extrajumps = extrajumpamount;
    }

    private enum State
    {
        Walking,
        Jumping,
    }
    private void PlayerMouvement()
    {
        moveinputX = Input.GetAxis("Horizontal");
        movedir = new Vector3(moveinputX, 0);

        PlayerCurrentSpeed = moveinputX * PlayerSpeed * Time.deltaTime;

        rb2D.velocity = new Vector2(PlayerCurrentSpeed, rb2D.velocity.y);
    }
    void MCjumping()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.UpArrow)) && extrajumps > 0)
        {
            state = State.Jumping;
            rb2D.velocity = Vector2.up * jumpforce;
            extrajumps--;
            print("Player is jumping");
        }
        
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.UpArrow)) && extrajumps == 0 && isgrounded == true)
        {
            rb2D.velocity = Vector2.up * jumpforce;
        }
        
        if (isgrounded)
            extrajumps = extrajumpamount;
    }

    void Jumpteaking()
    {
        //Jump tweaking (gravitu modifier)
        if (rb2D.velocity.y < 0)
        {
            //Rb2d.velocity += Rb2d.velocity * fallmultipliyertest;
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallmultipliyer - 1) * Time.deltaTime;
        }
        else if (rb2D.velocity.y > 0 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowjumpmultipliyer) * Time.deltaTime;
        }
    }

    private void GroundCheck()
    {
        isgrounded = Physics2D.OverlapCircle(groundcheck.position, groundcheckradius, whatisground);
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

    void Update()
    {
        PlayerMouvement();

        MCflip();
        GroundCheck();

        switch (state)
        {
            default:
            case State.Walking:
                //PLayer mouvement
                if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.UpArrow)) && extrajumps > 0)
                {
                    state = State.Jumping;
                }
                    break;

            case State.Jumping:
                rb2D.velocity = new Vector2(PlayerCurrentSpeed, rb2D.velocity.y);
                Jumpteaking();
                if (isgrounded)
                {
                    state = State.Walking;
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.Walking:
                //PLayer mouvement
                rb2D.velocity = new Vector2(PlayerCurrentSpeed, rb2D.velocity.y);    
                    
                MCjumping();
                break;
            
            case State.Jumping:
                rb2D.velocity = new Vector2(PlayerCurrentSpeed, rb2D.velocity.y);
                Jumpteaking();
                if(isgrounded)
                {
                    state = State.Walking;
                }
                break;
        }
    }
}
