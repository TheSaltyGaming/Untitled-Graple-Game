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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            print("Level1");
            RestartScene();
        }
    }

    private void RestartScene()
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
    
}