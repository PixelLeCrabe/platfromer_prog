using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller2D : MonoBehaviour
{
    private const float MOVE_SPEED = 60f;

    private Rigidbody2D rigidbody2D;
    private Vector3 moveDir;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
private void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }
       moveDir = new Vector3(moveX, moveY).normalized;
        {
           
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = moveDir * MOVE_SPEED;

    }
}
