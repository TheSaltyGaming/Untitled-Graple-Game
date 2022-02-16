using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class HelmetBounce : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Player detected");
            other.gameObject.GetComponent<TestMovement>().BounceUp();
        }
    }
}
