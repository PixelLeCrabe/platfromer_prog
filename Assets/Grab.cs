using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public static Grab instance;
    
    public Transform Holdpoint;
    public Transform RayCatsPoint;
    public Transform MC;

    private Collider2D GrabbedItemCollider;

    private Rigidbody2D GrabbedItem;
    public LayerMask GrabbableItem;

    private RaycastHit2D ray;

    public string GrabedItemName;

    public bool IsHoldingAgrabedItem;
    private bool CanGrab;
    private bool Grabbing;
    private bool CanRelease;
    private bool IsReleasingAnItem;


    public float Grabrange;
    [SerializeField] private float ThrowStrenght;
    private float nbofgrabInput;
    private float GrabedItemGravityAmount;
    private float HoldMaxTime;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        nbofgrabInput = 0;
        CanRelease = false;
        nbofgrabInput = 0;
    }
    private void Grabbleobject()
    {         
        //just changed dir vector                
        RaycastHit2D ray = Physics2D.Raycast(RayCatsPoint.position, new Vector2(MC.localScale.x, 0), Grabrange, GrabbableItem);
        Debug.DrawRay(new Vector2(RayCatsPoint.position.x, RayCatsPoint.position.y), new Vector2(MC.localScale.x, 0) * Grabrange, Color.red);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Grab"))
        {                 
            if (ray.collider != null && !Grabbing)
            {
                CanRelease = false;
                Grabbing = true;
                IsHoldingAgrabedItem = true;
                
                // TP the Gameobject
                GrabedItemGravityAmount = ray.collider.gameObject.GetComponent<Rigidbody2D>().gravityScale;
                ray.collider.gameObject.GetComponent<Rigidbody2D>().gravityScale =0;
                GrabbedItem = ray.collider.gameObject.GetComponent<Rigidbody2D>();
                
                // Test : Identify the grabed item by his name 
                GrabedItemName = ray.collider.gameObject.name;

                //testing can enable or disable collider here to Grab closer to the MC //  Give him back on release !
                GrabbedItemCollider = ray.collider.gameObject.GetComponent<Collider2D>();
                ray.collider.gameObject.GetComponent<Collider2D>().enabled = false;
                nbofgrabInput++;
                StartCoroutine(CoroutineRelease());
                Debug.Log(gameObject.name + " is grabbed");               
            }
            /*else
                Grabbing = false;*/
        }
    }

    private void ReleaseItem2()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetButtonDown("Grab")) && CanRelease && Grabbing)
        {
            //IsHoldingAgrabedItem = false;
            print("Grabbing"); 
            // Throw right
            if (Input.GetAxis("Horizontal") > 0)
            {
                IsHoldingAgrabedItem = false;
                GrabbedItem.AddForce(new Vector2(1, 0.1f) * ThrowStrenght, ForceMode2D.Impulse);
                GrabbedItem.gravityScale = GrabedItemGravityAmount;
                print(" Throw Item Right");
            }

            // Throw left
            else if (Input.GetAxis("Horizontal") < 0)
            {
                IsHoldingAgrabedItem = false;
                GrabbedItem.AddForce(new Vector2(-1, 0.1f) * ThrowStrenght, ForceMode2D.Impulse);
                GrabbedItem.gravityScale = GrabedItemGravityAmount;
                print(" Throw Item left");
            }

            else
            {
                IsHoldingAgrabedItem = false;
                GrabbedItem.gravityScale = GrabedItemGravityAmount;
                print("releasing an item with W");
            }

            //aled
            
            // Throw up
            if (Input.GetAxis("Vertical") > 0.1)
            {
                IsHoldingAgrabedItem = false;
                GrabbedItem.AddForce(new Vector2(0, 1) * ThrowStrenght, ForceMode2D.Impulse);
                GrabbedItem.gravityScale = GrabedItemGravityAmount;
                print(" Throw Item up");
            }
           
                // Throw Down
              else if (Input.GetAxis("Vertical") < 0)
            {
                IsHoldingAgrabedItem = false;
                GrabbedItem.AddForce(new Vector2(0, -1) * ThrowStrenght, ForceMode2D.Impulse);
                GrabbedItem.gravityScale = GrabedItemGravityAmount;
                print(" Throw Item Down");
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
        ReleaseItem2();
        
        //HoldingItem in fixed Update
    }
    private void FixedUpdate()
    {
        HoldinganItem();
    }

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
}
