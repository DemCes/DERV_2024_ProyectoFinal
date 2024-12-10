using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{

    [SerializeField] Barra_Vida barra_Vida;
    private bool isPlayerNearby = false; // Tracks if the player is in range
    public float maxDamage = 500f; // Adjusted to a more reasonable value
    public float minDamage = 300f; // Adjusted to a more reasonable value
    public float damageInterval = 0.2f;
    private float lastDamageTime;

    
    void Start()
    {
        if(barra_Vida == null)
        {
            Debug.LogWarning("Healthbar no esta asignada");
        }
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Jugador esta cerca del fuego");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Salio del area de fuego");
        }
    }
    void Update()
    {
        if(isPlayerNearby)
        {
            if(Time.time - lastDamageTime >= damageInterval)
            {
                float damage = Random.Range(minDamage, maxDamage);

                if(barra_Vida != null)
                {
                    barra_Vida.TakeDamage(damage);
                }
                lastDamageTime = Time.time;
            }
        }
    }
}
