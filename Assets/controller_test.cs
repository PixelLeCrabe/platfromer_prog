using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_test : MonoBehaviour
{ 
    // Animation Handler
    public string Currentstate;

    public const string PLAYER_IDLE = ("idle_double_mechant");
    public const string PLAYER_WALK = ("walking_cycle_double_mechant");
    public const string PLAYER_JUMP = ("jump");
    public const string PLAYER_DOUBLEJUMP = ("MC_double_jump");
    public const string PLAYER_FALLING_DOWN = ("MC_fallingDown");
    public const string PLAYER_LANDED   = ("MC_landed");
    public const string PLAYER_ROLLING = ("MC_roulade");

    public static controler2D_plateformingv2 instance;

    private enum State
    {
        Normal,
        Rolling,
    }

    public Animator animator;

    public LayerMask whatisground;

    private State state;

    private HPbar hpbar;

    [SerializeField] private Rigidbody2D Rb2d;

    public Transform groundcheck;

    private Vector2 movedir;
    private Vector3 RollDir;
    private Vector3 rolldirpos = new Vector3(1, 0, 0);
    private Vector3 rolldirneg = new Vector3(-1, 0, 0);
    private Vector3 CurrentPostion;

    private bool hasjumped;
    private bool isRollinginair;
    private bool isgrounded;
    private bool isdashbuttondown;
    private bool isDoubleJumping;
    private bool IsfallingDown;
    private bool IsRolling;
    private bool IsLanded;
    private bool CanRoll;

    [SerializeField] private float airRollamount;
    [SerializeField] private float RollSpeed;
    [SerializeField] private float checkradius;
    [SerializeField] private float lowjumpmultipliyer;
    [SerializeField] private float fallmultipliyer;
    [SerializeField] private float dashdistance;
    [SerializeField] private float speed;
    [SerializeField] private float jumpforce;
    [SerializeField] private float RollCD;
    [SerializeField] private float AnimationDelay;
    [SerializeField] private float RollAnimationDelay;
    [SerializeField] private float LandedAnimationDelay;
    private float airRoll;
    private float PlayerCurrentSpeed;
    private float LastMoveDir;
    private float moveinputX;
    private float rollspeedminimum;

    [SerializeField] private int extrajumpamount;
    private int extrajumps;

    private void Awake()
    {
        extrajumps = extrajumpamount;
        state = State.Normal;
        airRoll = airRollamount;
        hpbar = GetComponent<HPbar>();
        CanRoll = true;
    }
    public void PlayerAnimationState(string newState)
    {
        if (Currentstate == newState) return;

        animator.Play(newState);

        Currentstate = newState;
    }

    private void PlayerMouvement()
    {
        moveinputX = Input.GetAxis("Horizontal");
        movedir = new Vector3(moveinputX, 0);

        PlayerCurrentSpeed = moveinputX * speed * Time.deltaTime;

        if (Mathf.Abs(moveinputX) < .01f && isgrounded && !IsRolling && !IsLanded) 
        {
            // Animation
            PlayerAnimationState(PLAYER_IDLE);
        }
        else if (isgrounded && !IsfallingDown && !isRollinginair && !IsRolling && !IsLanded)
        {
            PlayerAnimationState(PLAYER_WALK);
        }
    }

    private void MCjumping()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.UpArrow)) && extrajumps > 0)
        {
            Rb2d.velocity = Vector2.up * jumpforce;
            extrajumps--;
            // Animation 
            PlayerAnimationState(PLAYER_JUMP);
        }
        //Aled

        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.UpArrow)) && extrajumps == 0 && isgrounded)
        {
            Rb2d.velocity = Vector2.up * jumpforce;
            PlayerAnimationState(PLAYER_JUMP);
        }

        if (!isgrounded && !IsfallingDown && !isDoubleJumping && !isRollinginair)
        {
            //animator.SetBool("isJumping", true);
            //PlayerAnimationState(PLAYER_JUMP);
        }
    }

    void DoubleJumpAnimation()
    {
        if (extrajumps == 0 && !isgrounded && !IsfallingDown && !isRollinginair)
        {
            PlayerAnimationState(PLAYER_DOUBLEJUMP);
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

    void Roll()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Fire3")) && airRoll >= 0 && CanRoll)
        {
            CanRoll = false;
            IsRolling = true;
            RollDir = movedir;
            state = State.Rolling;
            RollSpeed = 10f;
            airRoll--;
            //HPbar.instance.isinvisible = true;
            StopAllCoroutines();
            StartCoroutine(CoroutineInvisibleRollCD());
            StartCoroutine(CoroutineRollCD());
            StartCoroutine(CoroutineRollDelay());

            if (IsRolling)
            {
                PlayerAnimationState(PLAYER_ROLLING);
            }
        }

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
    }
    private void ExitRoll()
    {
        float rollspeeddropmultiplier = 3;
        RollSpeed -= RollSpeed * rollspeeddropmultiplier * Time.deltaTime;

        float rollspeedminimum = 7f;
        if (RollSpeed < rollspeedminimum)
        {
            state = State.Normal;
            //IsRolling = false;
        }
        isRollinginair = true;
    }

    // fixed update fonctions 
    private void DashPhysic()
    {
        // Missing an if() Input
        if (isdashbuttondown)
        {
            // Get 1 or -1 on x for where the MC is facing
            LastMoveDir = LastMoveDir / Mathf.Abs(LastMoveDir);

            CurrentPostion = new Vector3(transform.position.x + (LastMoveDir * dashdistance), transform.position.y);
            Rb2d.MovePosition(CurrentPostion);

            isdashbuttondown = false;
            CurrentPostion = transform.position;
        }

        Rb2d.velocity = new Vector2(PlayerCurrentSpeed, Rb2d.velocity.y);
    }
    private void DashLogic()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isdashbuttondown = true;
        }

        // Dash dir if Player is moving
        if (moveinputX != 0)
        {
            LastMoveDir = moveinputX;
        }
    }
    void fallingdown()
    {
        if (Rb2d.velocity.y < -0.01 && !isRollinginair && !isgrounded && !IsLanded)
        {
            IsfallingDown = true;
            PlayerAnimationState(PLAYER_FALLING_DOWN);
        }
        else
        {
            IsfallingDown = false;
        }
    }
    void Mchasjumpedonce()
    {
        if (Rb2d.velocity.y > .01)
        {
            hasjumped = true;
        }
    }

    void GroundedChecks()
    {
        if (isgrounded)
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

    // landed event Trigger
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Sol" && hasjumped == true)
        {
            IsLanded = true;
            StartCoroutine(CoroutineLandedlDelay());
            
            // Animation
            PlayerAnimationState(PLAYER_LANDED);
        }
    }

    void Update()
    {
        // Animation delay getting the animator timing
        //AnimationDelay = animator.GetCurrentAnimatorStateInfo(0).length - .1f;

        switch (state)
        {
            case State.Normal:

                PlayerMouvement();

                MCjumping();

                Jumpteaking();

                DoubleJumpAnimation();

                Roll();

                DashLogic();

                fallingdown();

                MCflip();
                Mchasjumpedonce();
                GroundedChecks();

                break;

            case State.Rolling:
                ExitRoll();
                //  HPbar.instance.isinvisible = true;
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

                //Player movement
                Rb2d.velocity = new Vector2(PlayerCurrentSpeed, Rb2d.velocity.y);

                DashPhysic();
                break;
            case State.Rolling:
                Rb2d.velocity = RollDir * RollSpeed;

                break;
        }
    }
    IEnumerator CoroutineInvisibleRollCD()
    {
        yield return new WaitForSeconds(1);
        //HPbar.instance.isinvisible = false;
    }

    IEnumerator CoroutineRollCD()
    {
        yield return new WaitForSeconds(RollCD);
        CanRoll = true;
    }

    IEnumerator CoroutineRollDelay()
    {
        yield return new WaitForSeconds(RollAnimationDelay);
        IsRolling = false;
    }

    IEnumerator CoroutineLandedlDelay()
    {
        yield return new WaitForSeconds(LandedAnimationDelay);
        IsLanded = false;
    }
}
