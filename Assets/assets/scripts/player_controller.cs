using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : physic_object
{
    private SpriteRenderer SpRenderer;
   
    //  /!\ don't edit here /!\
    public float jumpvelocity = 7f;
    
    //  /!\ don't edit here /!\
    public float  maxspeed = 7f;
    // Start is called before the first frame update
   
    void awake()
    {
        SpRenderer = GetComponent<SpriteRenderer> ();
    }

    protected override void Computevelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpvelocity;
        }

        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;

            }
        }
       
        //bool flipSprite = (SpRenderer.flipX ? ( move.x > 0.01f) : (move.x < -0.01f));
               //     if (flipSprite)
        {
           // SpRenderer.flipX = !SpRenderer.flipX;
        }
        
        targetvelocity = move * maxspeed;
    }
}
