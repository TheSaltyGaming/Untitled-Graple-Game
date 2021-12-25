using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    // Set to main camera
    public Camera cam;
    // Set to "Line"
    public LineRenderer lr;
    // What should grapple collide with. Set to ground
    public LayerMask grappleMask;
    public float moveSpeed = 2;
    public float grappleLength = 5;
    [SerializeField] private float grappleSpeedDivide = 2;
    [Min(1)]
    // Amount of points the grapple hook can have
    public int maxPoints = 3;

    private Input _input;
    private Movement _movement;
    private Collision _collision;
    private PlayerStateList pstate; 

    private Vector2 _mousePos;
    
    private Rigidbody2D _rigidbody2D;
    private List<Vector2> points = new List<Vector2>();

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _collision = GetComponent<Collision>();
        pstate = GetComponent<PlayerStateList>();
        lr.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_collision.IsGrounded())
        {
            pstate.justGrappled = false;
            pstate.initiateMovement = true;
        }
        
        _mousePos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
        
        // Initiates grapple
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            print("grapple registered");
            Vector2 mousePos = cam.ScreenToWorldPoint(_mousePos);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grappleLength, grappleMask);
            if (hit.collider != null)
            {
                Vector2 hitPoint = hit.point;
                points.Add(hitPoint);

                if (points.Count > maxPoints)
                {
                    points.RemoveAt(0);
                }   
            }
        }

        // If grappling
        if (points.Count > 0)
        {
            pstate.isGrappling = true;
            // Disables movement to prevent X velocity from being set to 0
            pstate.initiateMovement = false;
            Vector2 moveTo = centriod(points.ToArray());
            //_rigidbody2D.MovePosition(Vector2.MoveTowards((Vector2)transform.position, moveTo, Time.deltaTime * moveSpeed));
            Vector2 rotation = (moveTo - (Vector2)transform.position).normalized;
            _rigidbody2D.AddForce(rotation / grappleSpeedDivide, ForceMode2D.Impulse);

            lr.positionCount = 0;
            lr.positionCount = points.Count * 2;
            for (int n = 0, j=0; n < points.Count * 2; n+=2,j++)
            {
                lr.SetPosition(n, transform.position);
                lr.SetPosition(n+1, points[j]);
            }
        }
        else
        {
            pstate.isGrappling = false;
        }

        if(Keyboard.current.spaceKey.wasPressedThisFrame && pstate.isGrappling)
        {
            Detatch();
        }
    }

    public void Detatch()
    {
        lr.positionCount = 0;
        points.Clear();
        //_rigidbody2D.AddRelativeForce(_rigidbody2D.velocity, ForceMode2D.Impulse);
        
        //JustGrappled allows for the movement system to be enabled on A or D press
        pstate.justGrappled = true;
    }

    Vector2 centriod(Vector2[] points)
    {
        Vector2 center = Vector2.zero;
        foreach (Vector2 point in points)
        {
            center += point;
        }
        center /= points.Length;
        return center;
    }

    private void OnDrawGizmos()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(_mousePos);
        Vector2 direction =  (mousePos - (Vector2)transform.position).normalized;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position +  direction);

        foreach(Vector2 point in points)
        {
            Gizmos.DrawLine(transform.position, point);
        }
    }
}