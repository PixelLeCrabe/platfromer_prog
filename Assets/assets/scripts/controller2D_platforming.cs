using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller2D_platforming : MonoBehaviour
{
    public float VelocityAmount= 10f;
    public float MoveSpeed = 20f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
   
    [SerializeField] private Transform whatsisground; 
    
    private Vector3 Velocity = Vector3.zero;
    private Rigidbody2D rbody2D;
    private Vector2 moveDir;
    private Vector2 jumpjump;
    float Hzmove = 0f;
    public float JumpForce;
    private void Awake()
    {
        rbody2D = transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Vector3 targetVelocity = new Vector2(move, m_Rigidbody2D.velocity.y);
        // m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        //Hzmove = new Vector3();
        // Hzmove = Input.GetAxisRaw("Horizontal") * MoveSpeed;
        
        float MoveX = 0f;
        float MoveY = 0f;
        
        if (Input.GetKey(KeyCode.D))
        {
            MoveX = -1f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("left!");
            MoveX = +1f;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("jumping!"); 
            MoveY = +1f
                ;
        }

        MoveX = MoveX * VelocityAmount * Time.fixedDeltaTime;
        MoveY = MoveY * JumpForce * Time.fixedDeltaTime; 

        moveDir = new Vector2(MoveX, MoveY ).normalized;
        //jumpjump = new Vector2(0, MoveY);
    }

    private void FixedUpdate()
    {
        // moveDir = new Vector3(moveX, moveY).normalized;


        rbody2D.velocity = moveDir;
        
        
        //rbody2D.velocity = jumpjump * JumpForce;
        // if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.UpArrow))
        //{
        // rigidbody2D.AddForce(new Vector2(0f, JumpForce));
        //}

    }  // need to add a fonction that represent this line in the update fonction 

}
