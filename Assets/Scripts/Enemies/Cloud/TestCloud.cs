using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class TestCloud : MonoBehaviour
{
    [Range(0.1f, 2000f)]
    public float gravitation = 200.45f;
    [Range(4, 50)]
    public float gravitationRadius = 3.8f;
    [Range(0f, 10f)]
    public float rotationSpeed = 0.9f;

    private CircleCollider2D gravitationTrigger;
    [SerializeField] private List<Rigidbody2D> objectsInRange = new List<Rigidbody2D>();

    private Rigidbody2D _rb;
    private PlayerStateList _pState;
    private bool _savedPlayerState;
    void Start()
    {
        gravitationTrigger = GetComponent<CircleCollider2D>();
        gravitationTrigger.isTrigger = true;
        gravitationTrigger.radius = gravitationRadius / transform.localScale.x;
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed);

        foreach (var objectInVicinity in objectsInRange)
        {
            print(objectInVicinity);
            if (objectInVicinity == null) {
                objectsInRange.Remove(objectInVicinity);
                break;
            }

            float dist = Vector2.Distance(transform.position, objectInVicinity.transform.position);
            float gravitationFactor = 1 - dist / gravitationRadius;
            Vector2 force = (transform.position - objectInVicinity.transform.position).normalized * gravitation * gravitationFactor;
            objectInVicinity.AddForce(force);
            if (objectInVicinity.gameObject.CompareTag("Player"))
            {
                var pState = objectInVicinity.gameObject.GetComponent<PlayerStateList>();
            }
        }

        //disengage everything when grapple starts
        //make sure to cut grapple when player gets moved in the center please
        //this has not been done yet i don't think.
        if (_savedPlayerState)
        {
            if (_pState.isClouded && !_pState.isGrappling)
            {
                StuckInCloud();
            }
        }

    }

    public void StuckInCloud()
    {
        Vector2 cloudCenter = transform.GetChild(0).position;
        var newpos = Vector2.MoveTowards((Vector2) transform.position, cloudCenter, 0.00014f);
        _rb.MovePosition(newpos);
        gravitation = 0;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gravitationRadius);
    }

    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        print("cloudEnterTrigger");
        var rb = collider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            objectsInRange.Add(rb);
        }

        if (collider.CompareTag("Player"))
        {
            var grapple = collider.gameObject.GetComponent<Grapple>();
            var pState = collider.gameObject.GetComponent<PlayerStateList>();
            StartCoroutine(SlowDown(rb));
            _pState = pState;
            _savedPlayerState = true;

        }

    }

    private IEnumerator SlowDown(Rigidbody2D rb)
    {
        print("sdadadsadasdas");
        yield return new WaitForSeconds(0.3f);
        Vector2 cloudCenter = transform.GetChild(0).position;
        //var newpos = Vector2.MoveTowards((Vector2) transform.position, cloudCenter, 0.00014f);
        //rb.MovePosition(newpos);
        _rb = rb;

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        var rb = collider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            objectsInRange.Remove(rb);
        }
        
        gravitation = 600;
    }
}
