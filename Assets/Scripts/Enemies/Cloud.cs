using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private bool _clouded;
    //maybe do something with this bool?
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Trigger");
        if (other.CompareTag("Player"))
        {
            print("Cloud Trigger");
            var rb = other.gameObject.GetComponent<Rigidbody2D>();
            var grapple = other.gameObject.GetComponent<Grapple>();
            grapple.lr.positionCount = 0;
            grapple.points.Clear();
            other.gameObject.GetComponent<PlayerStateList>().canGrapple = true;
            other.gameObject.GetComponent<PlayerStateList>().isGrappling = false;
            while (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2((rb.velocity.x - 5) * Time.deltaTime, rb.velocity.y);
                if (rb.velocity.x < 3 && rb.velocity.y <3)
                {
                    
                }
                break;
            }
            while (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2((rb.velocity.x), rb.velocity.y -5 *Time.deltaTime);
                if (rb.velocity.x < 3 && rb.velocity.y <3)
                {
                    
                }
                break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var rb = other.gameObject.GetComponent<Rigidbody2D>();
            print("Sustain Trigger");
            if (!other.gameObject.GetComponent<PlayerStateList>().isGrappling)
            {
                other.gameObject.GetComponent<PlayerStateList>().initiateMovement = false;
                other.gameObject.GetComponent<PlayerStateList>().canGrapple = true;
                //rb.velocity = Vector2.zero;
                rb.gravityScale = 6;

                Vector2 cloudCenter = transform.GetChild(0).position;

                rb.MovePosition(Vector2.MoveTowards((Vector2)transform.position, cloudCenter, Time.deltaTime * 0.000002f));

                //this works if we set a point and keep moving the player even if they leave the trigger,
                //but that means we need "public gameObject player"
                //Vector2 rotation = (moveTo - (Vector2)transform.position).normalized;
                //rb.AddForce(rotation / 2, ForceMode2D.Impulse);
                
            }
            else
            {
                //rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;
                other.gameObject.GetComponent<PlayerStateList>().initiateMovement = false;
                rb.gravityScale = 6;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 6;
        //rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;

    }
}
