using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physic_object : MonoBehaviour
{

    public float gravitymodifier = 1f;
    public float minGroundNormalY = 0.65f;

    protected bool grounded;
    protected Vector2 groundNormal;

    protected Vector2 velocity;
    protected Vector2 targetvelocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected Rigidbody2D rigidbody2D;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minmovedist = 0.001f;
    protected const float shellradius = 0.01f;

    void OnEnable()
    {
        //recup le rigidbody set up dans unity
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Sets to filter contact results based on trigger collider involvement. Set to false to ignore any contacts involving trigger colliders
        contactFilter.useTriggers = false;
        //getting a layer mask from the project settings _ use the settings from the Physics 2D setting to know what layer u gonna checkfor collision  
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        //use the layermask set true instead of the triggers
        contactFilter.useLayerMask = true;
    }


    // Update is called once per frame
    void Update()
    {
        targetvelocity = Vector2.zero;

        Computevelocity ();
    }
        protected virtual void Computevelocity()
    {
        
    }

    void FixedUpdate ()
    {
        // Time.deltaTime to make the fct non frame dependant
        velocity += gravitymodifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetvelocity.x;

        grounded = false;

        //enable to have a vector matching the angle of the ground ( so you don't run into a pente)

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 movealongground = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = movealongground * deltaPosition.x;

        Movement (move, false);
        
        move = Vector2.up * deltaPosition.y;

        Movement (move, true);
    }
    void Movement (Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minmovedist)
        {
            //checking in front of you in the direction you moove if there is a collider ( via the ray cast)  / ( int count = ) to return the value 
            int count = rigidbody2D.Cast(move, contactFilter, hitBuffer, distance + shellradius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            
            for (int i = 0; i < hitBufferList.Count; i++)
            {            
                Vector2 currentNormal = hitBufferList[i].normal;
                //cheking the angle of the object ur collinding with ( so u don't get stuck in a wall )
                if (currentNormal.y > minGroundNormalY) ;
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }

                }
                //difference between the velocity and the current normal 
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0);
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifidistance = hitBuffer[i].distance - shellradius;
                distance = modifidistance < distance ? modifidistance : distance;
            }
        }

        rigidbody2D.position = rigidbody2D.position + move.normalized * distance;
         
    }
}

