using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpin : MonoBehaviour
{

    private bool _isRotated;
    public bool canSpin;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        canSpin = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canSpin)
            {
                print("spin");
                canSpin = false;
                _animator.Play("SpikeSPin");
                StartCoroutine(spinBack());
            }
        }
    }

    public IEnumerator spinBack()
    {
        yield return new WaitForSeconds(6f);
        _animator.Play("SpikeSPin");
    }

    public IEnumerator enableSpinCoroutine()
    {
        yield return new WaitForSeconds(6);
        canSpin = true;
    }
}
