using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inundacion : MonoBehaviour
{
    public float speed = 1.0f; // Velocidad de subida del agua
    public float maxHeight = 80.0f; // Altura máxima del agua

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (transform.position.y < maxHeight)
        {
            transform.position += Vector3.up * speed * Time.deltaTime; //esta aumentando la velocidad y rapidez con que crece Y
        }
    }

  
}
