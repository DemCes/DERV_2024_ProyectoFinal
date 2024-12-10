using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZonaTrigger : MonoBehaviour
{
    public string tagEsperado;
    public Objetivo1 gestorObjetivo;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Zona {gameObject.name}: Detectó entrada de {other.gameObject.name}");

        MultiplesTags multiplesTags = other.GetComponent<MultiplesTags>();
        if (multiplesTags == null)
        {
            multiplesTags = other.GetComponentInParent<MultiplesTags>();
        }

        if (multiplesTags != null && multiplesTags.HasTag(tagEsperado))
        {
            gestorObjetivo.NotificarEntradaSaco(tagEsperado);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MultiplesTags multiplesTags = other.GetComponent<MultiplesTags>();
        if (multiplesTags == null)
        {
            multiplesTags = other.GetComponentInParent<MultiplesTags>();
        }

        if (multiplesTags != null && multiplesTags.HasTag(tagEsperado))
        {
            gestorObjetivo.NotificarSalidaSaco(tagEsperado);
        }
    }
}