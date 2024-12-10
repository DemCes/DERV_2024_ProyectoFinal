using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeManager : MonoBehaviour
{
    [SerializeField] private Barra_Vida barraVida;  // Reference to health bar
    [SerializeField] private float minTime,maxTime; 

    private void Start() {
        if (Safe_Zone_Manager.Instance != null)
        {
            Debug.Log("Singleton funciona");
        }
        if (barraVida == null)Debug.Log("Vida no asignada");
    }
    private void Update(){
        
    }

    
}
