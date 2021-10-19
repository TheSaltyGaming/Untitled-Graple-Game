using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStateList))]
[RequireComponent(typeof(Input))]
public class TestMovement : MonoBehaviour
{
    public PlayerStateList pState;
    
    [Header("X Axis Movement")]
    [SerializeField] float walkSpeed = 25f;
 
    [Space(5)]
 
    [Header("Y Axis Movement")]
    [SerializeField] float jumpSpeed = 45;
    [SerializeField] float fallSpeed = 45;
    [SerializeField] int jumpSteps = 20;
    [SerializeField] int jumpThreshold = 7;
    [Space(5)]

    [Header("Roof Checking")]
    [SerializeField] Transform roofTransform; //This is supposed to be a transform childed to the player just above their collider.
    [SerializeField] float roofCheckY = 0.2f;
    [SerializeField] float roofCheckX = 1; // You probably want this to be the same as groundCheckX
    [Space(5)]
    
    
    float xAxis;
    float yAxis;
    int stepsXRecoiled;
    int stepsYRecoiled;
    int stepsJumped = 0;
    float grabity;
    Rigidbody2D rb;

    private Input _input;
    private Collision _collision;
    
    void Start () {
        if(pState == null)
        {
            pState = GetComponent<PlayerStateList>();
        }
 
        rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<Input>();
        _collision = GetComponent<Collision>();
 
        grabity = rb.gravityScale;
    }
    
    void Update () {
        Flip();
        Walk(xAxis);
        GetInputs();
        Jump();
    }
    void Jump()
    {
        if (pState.jumping)
        {
            if (stepsJumped < jumpSteps && !Roofed())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                stepsJumped++;
            }
            else
            {
                StopJumpSlow();
            }
        }
    }
    void Walk(float MoveDirection)
    {
        //Rigidbody2D rigidbody2D = rb;
        //float x = MoveDirection * walkSpeed;
        //Vector2 velocity = rb.velocity;
        //rigidbody2D.velocity = new Vector2(x, velocity.y);
        if (!pState.justGrappled && !pState.isGrappling)
        {
            print("STOPTHEFUCK");
            rb.velocity = new Vector2(MoveDirection * walkSpeed, rb.velocity.y);   
        }

        if (Mathf.Abs(rb.velocity.x) > 0)
        {
                pState.walking = true;
        }
        else
        {
                pState.walking = false;
        }
        if (xAxis > 0)
        {
            pState.lookingRight = true;
        }
        else if (xAxis < 0)
        {
            pState.lookingRight = false;
        }
 
            //anim.SetBool("Walking", pState.walking);

    }

    void Flip()
    {
        if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        else if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
    }
    
    void StopJumpQuick()
    {
        //Stops The player jump immediately, causing them to start falling as soon as the button is released.
        stepsJumped = 0;
        pState.jumping = false;
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }
 
    void StopJumpSlow()
    {
        //stops the jump but lets the player hang in the air for awhile.
        stepsJumped = 0;
        pState.jumping = false;
    }

    public bool Roofed()
    {
        //This does the same thing as grounded but checks if the players head is hitting the roof instead.
        //Used for canceling the jump.
        if (Physics2D.Raycast(roofTransform.position, Vector2.up, roofCheckY, _collision.whatIsGround) || Physics2D.Raycast(roofTransform.position + new Vector3(roofCheckX, 0), Vector2.up, roofCheckY, _collision.whatIsGround) || Physics2D.Raycast(roofTransform.position + new Vector3(roofCheckX, 0), Vector2.up, roofCheckY, _collision.whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GetInputs()
    {
        yAxis = _input.moveVector.y;
        xAxis = _input.moveVector.x;
 
        //This is essentially just sensitivity.
        if (yAxis > 0.25)
        {
            yAxis = 1;
        }
        else if (yAxis < -0.25)
        {
            yAxis = -1;
        }
        else
        {
            yAxis = 0;
        }
 
        if (xAxis > 0.25)
        {
            xAxis = 1;
        }
        else if (xAxis < -0.25)
        {
            xAxis = -1;
        }
        else
        {
            xAxis = 0;
        }
        if (_input.jump && _collision.IsGrounded())
        {
            pState.jumping = true;
        }

        if (!_input.jump && stepsJumped < jumpSteps && stepsJumped > jumpThreshold && pState.jumping)
        {
            StopJumpQuick();
        }
        else if (!_input.jump && stepsJumped < jumpThreshold && pState.jumping)
        {
            StopJumpSlow();
        }
    }
    
}
