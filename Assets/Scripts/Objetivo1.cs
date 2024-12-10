using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objetivo1 : MonoBehaviour
{
    [Header("Cambio de Escena")]
    public CambioEscena panelCambioEscena;

    [Header("Configuraci�n de Tags")]
    public string tagSacoGrande = "SacoGrande";
    public string tagSacoChico = "SacoChico";

    [Header("Referencias")]
    public GameObject sacoGrande;
    public GameObject sacoChico;
    public BoxCollider triggerZonaSacoGrande;
    public BoxCollider triggerZonaSacoChico;

    [Header("UI")]
    public TextMeshProUGUI textoObjetivo;
    public Canvas objetivoCanvas;
    public GameObject panelCompletado;

    private bool sacoGrandeSobreTrigger = false;
    private bool sacoChicoSobreTrigger = false;

    void Start()
    {
        Debug.Log("=== Iniciando Sistema de Objetivos ===");
        ValidarConfiguracion();
        ConfigurarSacos();
        ConfigurarZonasTrigger();

        if (textoObjetivo != null)
        {
            Debug.Log("Texto objetivo activado al inicio.");
        }

        if (panelCompletado != null)
        {
            panelCompletado.gameObject.SetActive(false);
        }
    
}

    private void ConfigurarZonasTrigger()
    {
        ConfigurarZonaTrigger(triggerZonaSacoGrande.gameObject, tagSacoGrande, "Zona Saco Grande");
        ConfigurarZonaTrigger(triggerZonaSacoChico.gameObject, tagSacoChico, "Zona Saco Chico");
    }

    private void ConfigurarZonaTrigger(GameObject zona, string tag, string nombre)
    {
        ZonaTrigger zonaTrigger = zona.GetComponent<ZonaTrigger>();
        if (zonaTrigger == null)
        {
            zonaTrigger = zona.AddComponent<ZonaTrigger>();
        }
        zonaTrigger.tagEsperado = tag;
        zonaTrigger.gestorObjetivo = this;
        Debug.Log($"Zona {nombre} configurada para esperar tag: {tag}");
    }

    public void NotificarEntradaSaco(string tag)
    {
        if (tag == tagSacoGrande)
        {
            sacoGrandeSobreTrigger = true;
            Debug.Log("Saco Grande en posici�n correcta");
        }
        else if (tag == tagSacoChico)
        {
            sacoChicoSobreTrigger = true;
            Debug.Log("Saco Chico en posici�n correcta");
        }

        VerificarObjetivo();
    }

    public void NotificarSalidaSaco(string tag)
    {
        if (tag == tagSacoGrande)
        {
            sacoGrandeSobreTrigger = false;
            Debug.Log("Saco Grande fuera de posici�n");
        }
        else if (tag == tagSacoChico)
        {
            sacoChicoSobreTrigger = false;
            Debug.Log("Saco Chico fuera de posici�n");
        }
    }

    public bool EstanSacosColocados()
    {
        return sacoGrandeSobreTrigger && sacoChicoSobreTrigger;
    }

    private void VerificarObjetivo()
    {
        if (EstanSacosColocados())
        {
            Debug.Log("�Objetivo completado!");
            if (objetivoCanvas != null)
            {
                textoObjetivo.text = "Tapa la entrada principal y el drenaje con los sacos de arena (completado)";
            }
            StartCoroutine(MostrarPanelCompletado());

            var objetivo2 = FindObjectOfType<Objetivo2>();
            if (objetivo2 != null && objetivo2.EstanTodosObjetosRevelados())
            {
                if (panelCambioEscena != null)
                {
                    panelCambioEscena.CompletarObjetivos();
                }
                panelCambioEscena?.CompletarObjetivos();
            }
        }
    }

    private IEnumerator MostrarPanelCompletado()
    {
        if (panelCompletado != null)
        {
            panelCompletado.SetActive(true);
            yield return new WaitForSeconds(10f);
            panelCompletado.SetActive(false);
        }
    }


private void ConfigurarSacos()
    {
        ConfigurarSaco(sacoGrande, "Saco Grande", tagSacoGrande);
        ConfigurarSaco(sacoChico, "Saco Chico", tagSacoChico);
    }

    private void ConfigurarSaco(GameObject saco, string nombreSaco, string tag)
    {
        if (saco == null)
        {
            Debug.LogError($"Falta asignar el {nombreSaco} en el inspector!");
            return;
        }

        Rigidbody rb = saco.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = saco.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false;
        rb.useGravity = true;

        BoxCollider col = saco.GetComponent<BoxCollider>();
        if (col == null)
        {
            col = saco.AddComponent<BoxCollider>();
        }

        MultiplesTags tags = saco.GetComponent<MultiplesTags>();
        if (tags == null)
        {
            tags = saco.AddComponent<MultiplesTags>();
        }

        Debug.Log($"{nombreSaco} configurado: Rigidbody={rb != null}, Collider={col != null}, MultiplesTags={tags != null}");

        if (!tags.HasTag(tag))
        {
            tags.AddTag(tag);
            Debug.Log($"Tag '{tag}' a�adido al {nombreSaco}");
        }
    }

    private void ValidarConfiguracion()
    {
        Debug.Log("Verificando configuraci�n del sistema...");

        if (triggerZonaSacoGrande == null)
            Debug.LogError("Falta asignar la zona trigger para el Saco Grande");
        else
        {
            triggerZonaSacoGrande.isTrigger = true;
            Debug.Log($"Zona Saco Grande tabien - Posici�n: {triggerZonaSacoGrande.transform.position}");
        }

        if (triggerZonaSacoChico == null)
            Debug.LogError("Falta asignar la zona trigger para el Saco Chico");
        else
        {
            triggerZonaSacoChico.isTrigger = true;
            Debug.Log($"Zona Saco Chico tabien - Posici�n: {triggerZonaSacoChico.transform.position}");
        }
    }
}