using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuerzaPared : MonoBehaviour
{
   
    public float forceMagnitude = 10f; // Magnitud de la fuerza

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Aplica la fuerza en la dirección opuesta
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 forceDirection = (contactPoint - transform.position).normalized;

            Vector3 force = -forceDirection * forceMagnitude;

            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}