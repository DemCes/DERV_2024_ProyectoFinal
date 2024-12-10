using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P3_TomarObjetosV2 : MonoBehaviour
{
    bool objetoCerca;
    GameObject objetoActual;
    bool objetoTomada;
    private Animator animator;

    void Start()
    {
        // Intentar obtener el Animator del padre
        animator = GetComponentInParent<Animator>();

        // Si no se encuentra el Animator, mostrar un warning en la consola
        if (animator == null)
        {
            Debug.LogWarning("No se encontró un Animator en el objeto padre. Las animaciones no funcionarán.");
        }
    }

    void Update()
    {
        if (objetoCerca)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!objetoTomada)
                {
                    // Tomar el objeto
                    objetoActual.transform.SetParent(transform);
                    Rigidbody rb = objetoActual.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    objetoActual.transform.position = transform.position;
                    objetoActual.transform.rotation = transform.rotation;
                    objetoTomada = true;

                    // Activar la animación de cargar solo si existe el Animator
                    if (animator != null)
                    {
                        animator.Play("Carrying");
                    }
                }
                else
                {
                    // Soltar el objeto manteniendo su posición X y Z actual
                    Vector3 currentPosition = objetoActual.transform.position;
                    objetoActual.transform.position = new Vector3(currentPosition.x, 69.91f, currentPosition.z);

                    objetoActual.transform.SetParent(null);
                    Rigidbody rb = objetoActual.GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    objetoTomada = false;

                    // Volver a la animación por defecto solo si existe el Animator
                    if (animator != null)
                    {
                        animator.Play("Idle");
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if ((obj.CompareTag("ObjetoTomable") || obj.name.Contains("SacoChico")) && !objetoTomada)
        {
            objetoCerca = true;
            objetoActual = obj;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        if ((obj.CompareTag("ObjetoTomable") || obj.name.Contains("SacoChico")) && obj == objetoActual && !objetoTomada)
        {
            objetoCerca = false;
            objetoActual = null;
        }
    }
}