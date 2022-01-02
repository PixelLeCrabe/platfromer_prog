using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public string Currentstate;

    public const string PLAYER_IDLE = ("idle_double_mechant");
    public const string PLAYER_WALK = ("walking_cycle_double_mechant");
    public const string PLAYER_ROLLING = ("MC_roulade");

    // Update is called once per frame

    public void PlayerAnimationState(string newState)
    {
        if (Currentstate == newState) return;
        
        animator.Play(newState);
        
        Currentstate = newState;
    }

    public void PlayerMovement()
    {
        if(Input.GetAxis("Horizontal") != 0 )
        {
            print("is moving right");
            PlayerAnimationState(PLAYER_WALK);
        }
    }

    public void PlayerRoll()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            print("is Jumping");
            PlayerAnimationState(PLAYER_ROLLING);
        }
    }
    public void PlayerInIdle()
    {
       if (Input.GetAxisRaw("Horizontal") < .01f && Input.GetAxisRaw("Horizontal") > -.01f)
        {
            print("is idle");
            PlayerAnimationState(PLAYER_IDLE);
        }
    }
    void Update()
    {
        PlayerInIdle();
        PlayerRoll();
        PlayerMovement();
    }
}
