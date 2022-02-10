using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerStateList))]
[RequireComponent(typeof(Input))]
public class TestMovement : MonoBehaviour
{
    public PlayerStateList pState;
    private Grapple _grapple;
    
    [Header("X Axis Movement")]
    [SerializeField] float walkSpeed = 25f;
 
    [Space(5)]
 
    [Header("Y Axis Movement")]
    [SerializeField] float jumpSpeed = 45;
    [SerializeField] float fallSpeed = 45;
    [SerializeField] int jumpSteps = 20;
    [SerializeField] int jumpThreshold = 7;
    [SerializeField] private float maxVelocityPos = 55f;
    [SerializeField] private float maxVelocityNeg = -55f;
    [Space(5)]

    [Header("Roof Checking")]
    [SerializeField] Transform roofTransform; //Empty game object attatched to player. Place right above player
    [SerializeField] float roofCheckY = 0.2f;
    [SerializeField] float roofCheckX = 1; // Same as groundCheckX
    [Space(5)]
    
    //Dashing
    [Header("Dashing")]
    [SerializeField] private float dashForce = 6f;
    public float cooldown = 0.7f;
    
    //TESTING AREA

    public TMP_Text velocityTest;
    
    
    float xAxis;
    float yAxis;
    int stepsXRecoiled;
    int stepsYRecoiled;
    int stepsJumped = 0;
    Rigidbody2D rb;

    private Input _input;
    private Collision _collision;

    private Vector2 lateVelocity;
    
    void Start () {
        _grapple = GetComponent<Grapple>();
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<Input>();
        _collision = GetComponent<Collision>();
        pState.canDash = true;
    }
    
    void Update () {
        Flip();
        Walk(xAxis);
        GetInputs();
        Jump();
        enableMovement();

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && pState.canDash)
        {
            StartCoroutine(Dash());
        }
        //print("Player velocity: "+ rb.velocity);
        velocityTest.text = "Player velocity: " + rb.velocity;
    }

    private void LateUpdate()
    {
        lateVelocity = rb.velocity;
    }

    private void FixedUpdate()
    {
        SetMaxVelocity();
    }

    // Enables movement after grapple 
    void enableMovement()
    {
        if (pState.justGrappled)
        {
            if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
            {
                pState.initiateMovement = true;
                pState.justGrappled = false;
            }
        }
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
        if (!pState.isGrappling && !pState.justGrappled && pState.initiateMovement)
        {
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

    }

    //Flips playermodel
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
        //Stop player jump immediatley
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

    //Roof check
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
    
    private void SetMaxVelocity()
    {
        if (_collision.IsGrounded()) return;
        if (rb.velocity.y < maxVelocityNeg)
        {
            print("maxvelocity triggered");
            rb.velocity = new Vector2(rb.velocity.x, maxVelocityNeg);
        }
        if (rb.velocity.x < maxVelocityNeg)
        {
            print("maxvelocity triggered");
            rb.velocity = new Vector2(maxVelocityNeg, rb.velocity.y);
        }
        
        
        if (rb.velocity.y > maxVelocityPos)
        {
            print("maxvelocity triggered");
            rb.velocity = new Vector2(rb.velocity.x, maxVelocityPos);
        }
        if (rb.velocity.x > maxVelocityPos)
        {
            print("maxvelocity triggered");
            rb.velocity = new Vector2(maxVelocityPos, rb.velocity.y);
        }
    }

    void GetInputs()
    {
        yAxis = _input.moveVector.y;
        xAxis = _input.moveVector.x;
        
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
    
    private IEnumerator Dash()
    {
        print("Dash registered");
        pState.isDashing = true;
        pState.initiateMovement = false;
        pState.justGrappled = true;
        if (_input.moveVector.magnitude > 1)
        {
            _input.moveVector = _input.moveVector.normalized;
        }
        rb.velocity += _input.moveVector * dashForce;
        print(rb.velocity + " Is the current dash velocity");
        yield return new WaitForSeconds(0.3f);
        pState.isDashing = false;
        StartCoroutine(DashCooldown());
        StartCoroutine(ReduceDashSpeed());
    }
    
    //Reduce dash speed after grappling
    private IEnumerator ReduceDashSpeed()
    {
        print("Should reduce maybe");
        while ((rb.velocity.magnitude/2) > 28)
        {
            print("Reducing dash speed now");
            //This only cuts X speed, but it might work for now
            //rb.velocity -= new Vector2((rb.velocity.x - 6) * Time.deltaTime, rb.velocity.y);
            
            rb.velocity -= new Vector2((rb.velocity.x - 3) * Time.deltaTime, rb.velocity.y -1 *Time.deltaTime);
        }
        yield return null;
    }

    private IEnumerator DashCooldown()
    {
        pState.canDash = false;
        yield return new WaitForSeconds(cooldown);
        pState.canDash = true;
    }

    public void BounceBack()
    {
        rb.velocity = Vector2.zero;
        pState.justGrappled = true;
        pState.initiateMovement = false;
        rb.velocity = -lateVelocity * 2;
    } 

    public void TestBounce(Collision2D bounceobject)
    {
        pState.justGrappled = true;
        pState.initiateMovement = false;
        
        var heading = bounceobject.transform.position - transform.position;
        var distance = heading.magnitude;
        var _direction = heading / distance;
        _grapple.lr.positionCount = 0;
        _grapple.points.Clear();
        rb.velocity = Vector2.zero;
        rb.velocity = _direction * -38;
        //RaycastHit2D hit = Physics2D.Raycast(bounceobject.transform.position, transform.position);
    }
}
