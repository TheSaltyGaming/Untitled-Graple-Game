using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Input))]
public class Movement : MonoBehaviour
{
    [Header("Private Variables")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float downSpeed = 4f;
    [SerializeField] private float maxVelocity = 24f;

    /* COMPONENTS */
    private Input _Input;
    private Rigidbody2D _Rigidbody2D;
    private Collision _collision;

    /* LONG JUMPING */
    private bool isJumping = false;
    private float jumpTimeCounter;
    private float jumpTime = 0.25f;

    // DOUBLE JUMP
    private int jumps = 1;
    [SerializeField] private int baseJumps = 1;
    
    /*COYOTE TIME*/
    [SerializeField] private float coyoteTime = 0.5f;
    private float coyoteTimeCounter;
    [HideInInspector] public bool canCoyote;

    [HideInInspector] public bool isGrappling;
    [HideInInspector] public bool justGrappled;

    private void Start()
    {
        _Input = GetComponent<Input>();
        _Rigidbody2D = GetComponent<Rigidbody2D>();
        _collision = GetComponent<Collision>();
    }

    private void Update()
    {
        //LongJump();
        //DownDash();
        SetMaxVelocity();
        


        print(canCoyote + "Can coyote?");
    
        if (_collision.IsGrounded())
        {
            canCoyote = true;
            jumps = baseJumps;
            coyoteTimeCounter = coyoteTime;
            justGrappled = false;
        }
        else if (_Rigidbody2D.velocity.y <= 0)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        else
        {
            canCoyote = false;
        }

        if (coyoteTimeCounter < 0)
        {
            canCoyote = false;
        }

        if (_Input.jump)
        {
            if (!canCoyote && jumps <=0) return;

            isJumping = true;
                jumpTimeCounter = jumpTime;
                jumps--;
                _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, jumpForce);
        }
    }

    private void LongJump()
    {
        if (_Input.longJump && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                _Rigidbody2D.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (!_Input.longJump)
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (justGrappled || isGrappling) return;
        _Rigidbody2D.velocity = new Vector2(_Input.moveVector.x * moveSpeed, _Rigidbody2D.velocity.y);
    }
    
    /*private void DownDash()
    {
        if (_Input.downDash)
        {
            _Rigidbody2D.AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);
        }
    }
*/
    
    private void SetMaxVelocity()
    {
        //if (_collision.IsGrounded()) return;
        if (Mathf.Abs(_Rigidbody2D.velocity.y) > maxVelocity)
        {
            bool negativeVelocity = _Rigidbody2D.velocity.y < 0;
            _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, (negativeVelocity) ? -maxVelocity : maxVelocity);
        }
        if (Mathf.Abs(_Rigidbody2D.velocity.x) > maxVelocity)
        {
            bool negativeVelocity = _Rigidbody2D.velocity.x < 0;
            _Rigidbody2D.velocity = new Vector2((negativeVelocity) ? -maxVelocity : maxVelocity, _Rigidbody2D.velocity.y);
        }
    }

}