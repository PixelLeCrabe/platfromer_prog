using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public static Grab instance;

    public Animator animator;
    public Transform Holdpoint;
    public Transform RayCatsPoint;
    public Transform MC;

    private controller_test Controller_Test;

    private Collider2D GrabbedItemCollider;

    private Rigidbody2D GrabbedItem;
 
    public LayerMask GrabbableItem;

    private RaycastHit2D ray;

    public string GrabedItemName;

    public bool IsHoldingAgrabedItem;
    public bool Grabbing;
    public bool GrabAnim;
    private bool CanRelease;
    public bool ItemIsThrown;

    public float Grabrange;
    [Header("Trow Parameters")]
        
    [SerializeField] private float ThrowStrenght;
    [SerializeField] private float GrabTriggerDelay;
    [SerializeField] private float GrabAnimation;
    private float nbofgrabInput;
    private float GrabedItemGravityAmount;
    private float HoldMaxTime;  
    
    public const string PLAYER_GRABING = ("King_Grab");
    public const string PLAYER_THROWING = ("King_Throw");
    public const string PLAYER_REALESING = ("King_Release");

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Controller_Test = GetComponent<controller_test>();
        nbofgrabInput = 0;
        CanRelease = false;
        nbofgrabInput = 0;
    }
    private void Grabbleobject()
    {    
        // Grab Range raycast
        RaycastHit2D ray = Physics2D.Raycast(RayCatsPoint.position, new Vector2(MC.localScale.x, 0), Grabrange, GrabbableItem);
        Debug.DrawRay(new Vector2(RayCatsPoint.position.x, RayCatsPoint.position.y), new Vector2(MC.localScale.x, 0) * Grabrange, Color.red);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Grab"))
        {     
            // Animation
            GrabAnim = true;
            StartCoroutine(CoroutineGrabAnimation());

            if (ray.collider != null && !IsHoldingAgrabedItem)
            {
                CanRelease = false;
                Grabbing = true;
                IsHoldingAgrabedItem = true;

                // TP the Gameobject
                // grabed item initial gravity
                 GrabedItemGravityAmount = ray.collider.gameObject.GetComponent<Rigidbody2D>().gravityScale;
                
                // Set the gravity of the object to 0
                 ray.collider.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

                // Rigidbody2D of the object 
                 GrabbedItem = ray.collider.gameObject.GetComponent<Rigidbody2D>();

                // Test : Identify the grabed item by his name 
                 GrabedItemName = ray.collider.gameObject.name;

                //testing can enable or disable collider here to Grab closer to the MC //  Give him back on release !
                GrabbedItemCollider = ray.collider.gameObject.GetComponent<Collider2D>();
                ray.collider.gameObject.GetComponent<Collider2D>().enabled = false;
                nbofgrabInput++;

                StartCoroutine(CoroutinereTriggerDelay());
                StartCoroutine(CoroutineRelease());
                //Debug.Log(gameObject.name + " is grabbed");               
            }
        }
    }

    private void ThrowItem(int throwdirx, float ThrowDiry)
    {
        IsHoldingAgrabedItem = false;
        GrabbedItem.AddForce(new Vector2(throwdirx, ThrowDiry) * ThrowStrenght, ForceMode2D.Impulse);
        GrabbedItem.gravityScale = GrabedItemGravityAmount;

        // Item is thrown 
        ItemIsThrown = true;
    }

    private void ReleaseItem()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Grab")) && CanRelease && IsHoldingAgrabedItem)
        {         
            //print("Grabbing"); 
            // Throw right
            if (Input.GetAxis("Horizontal") > 0)
            {
                ThrowItem(1, .1f);
                print(" Throw Item Right");

                // Animation
                //Controller_Test.PlayerAnimationState(PLAYER_THROWING);
            }

            // Throw left
            else if (Input.GetAxis("Horizontal") < 0)
            {
                ThrowItem(-1, .1f);

                //print(" Throw Item left");

                // Animation
                //Controller_Test.PlayerAnimationState(PLAYER_THROWING);
            }

            else
            {
                IsHoldingAgrabedItem = false;
                GrabbedItem.gravityScale = GrabedItemGravityAmount;
                //print("releasing an item with W");

                // Animation
                //Controller_Test.PlayerAnimationState(PLAYER_REALESING);
            }

            //aled

            // Throw up
            if (Input.GetAxis("Vertical") > 0.1)
            {
                ThrowItem(0, 1);

                //print(" Throw Item up");

                // Animation
                //Controller_Test.PlayerAnimationState(PLAYER_THROWING);
            }
           
                // Throw Down
              else if (Input.GetAxis("Vertical") < 0)
            {
                ThrowItem(0, -1f);
                print(" Throw Item Down");

                // Animation
                //Controller_Test.PlayerAnimationState(PLAYER_THROWING); 
            }
           
            // Enable grabed item collider not use, could be useful
            StartCoroutine(CoroutinereEnableCollider());
            Grabbing = false;
        }       
    }
    private void HoldinganItem()
    {
        if (IsHoldingAgrabedItem)
        {
            GrabbedItem.position = Holdpoint.position;
        }
    }
    void Update()
    {
        Grabbleobject();
        ReleaseItem();
        
        //HoldingItem in fixed Update
    }
    private void FixedUpdate()
    {
        HoldinganItem();
    }

    #region Coroutines
    IEnumerator CoroutineRelease()
    {
        yield return new WaitForSeconds(1f);
        CanRelease = true;
    }

    IEnumerator CoroutineHoldMaxTime()
    {
        yield return new WaitForSeconds(HoldMaxTime);
        IsHoldingAgrabedItem = false;
        print("Released item for holding it too long");
    }

    IEnumerator CoroutinereEnableCollider()
    {
        yield return new WaitForSeconds(.1f);
        GrabbedItemCollider.enabled = true;       
    }

    IEnumerator CoroutinereTriggerDelay()
    {
        yield return new WaitForSeconds(GrabTriggerDelay);
        Grabbing = false;
    }
    IEnumerator CoroutineGrabAnimation()
    {
        yield return new WaitForSeconds(GrabAnimation);
        GrabAnim = false;
    }
    #endregion
}
