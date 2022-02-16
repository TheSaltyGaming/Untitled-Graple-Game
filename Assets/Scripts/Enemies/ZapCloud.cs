using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapCloud : MonoBehaviour
{
    private bool _canZap;
    [SerializeField] private float zapCooldown = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _canZap)
        {
            
        }
    }

    private IEnumerator ZapCooldown()
    {
        _canZap = false;
        yield return new WaitForSeconds(zapCooldown);
        _canZap = true;
    }
}
