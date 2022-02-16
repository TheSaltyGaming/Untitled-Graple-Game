using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RoomCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _vcam;
    // Start is called before the first frame update
    void Start()
    {
        _vcam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("TriggerEnter");
            _vcam.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _vcam.enabled = false;
        }
    }
}
