using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCloudArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("child object trigger detected");
        if (other.gameObject.CompareTag("Player"))
        {
            print("player detected");
            var grapple = other.gameObject.GetComponent<Grapple>();
            grapple.lr.positionCount = 0;
            grapple.points.Clear();

            //JustGrappled allows for the movement system to be enabled on A or D press
            var pState = other.gameObject.GetComponent<PlayerStateList>();
            pState.justGrappled = true;
            pState.canGrapple = true;
            var parentCloud = GetComponentInParent<TestCloud>();
            parentCloud.StuckInCloud();
            pState.isClouded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pState = other.gameObject.GetComponent<PlayerStateList>().isClouded = false;
    }
}
