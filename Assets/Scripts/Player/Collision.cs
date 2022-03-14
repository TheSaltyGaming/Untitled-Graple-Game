using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsDeath;
    public LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance = 1.1f;
    private TestMovement _movement;

    private Vector2 dir;
    
    Rigidbody2D rb;
    
    
    //TEMPORARY FOR ANIMATIONS RIGHT NOW

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<TestMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            print("Level1");
            _animator.Play("TestPlayerDeath");
        }
    }

    public void RestartScene()
    {
        //SceneManager.LoadScene("SampleScene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        print("Level2");
    }
    
    public bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down, new Color(1f, 0f, 1f));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bounce"))
        {
            //_movement.BounceBack();
           _movement.TestBounce(other);
        }
    }
}