using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lr;
    public LayerMask grappleMask;
    public float moveSpeed = 2;
    public float grappleLength = 5;
    [Min(1)]
    public int maxPoints = 3;

    private Input _input;

    private Vector2 _mousePos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
    
    private Rigidbody2D _rigidbody2D;
    private List<Vector2> points = new List<Vector2>();

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        lr.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
        if (_input.grapple)
        {
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

        if (points.Count > 0)
        {
            Vector2 moveTo = centriod(points.ToArray());
            _rigidbody2D.MovePosition(Vector2.MoveTowards((Vector2)transform.position, moveTo, Time.deltaTime * moveSpeed));

            lr.positionCount = 0;
            lr.positionCount = points.Count * 2;
            for (int n = 0, j=0; n < points.Count * 2; n+=2,j++)
            {
                lr.SetPosition(n, transform.position);
                lr.SetPosition(n+1, points[j]);
            }
        }

        if(_input.detatch)
        {
            Detatch();
        }
    }

    public void Detatch()
    {
        lr.positionCount = 0;
        points.Clear();
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