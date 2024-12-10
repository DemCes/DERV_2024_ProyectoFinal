using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string nombreSiguienteEscena;
    [SerializeField] private float distanciaInteraccion = 2f;

    private GameObject panelCompleto;
    private bool objetivosCompletados = false;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        panelCompleto = this.gameObject;

        if (panelCompleto != null)
        {
            panelCompleto.SetActive(false);
        }

        if (string.IsNullOrEmpty(nombreSiguienteEscena))
        {
            Debug.LogError("Nombre de la siguiente escena no configurado");
        }
    }

    private void Update()
    {
        if (!objetivosCompletados || player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        if (distancia <= distanciaInteraccion && Input.GetKeyDown(KeyCode.E))
        {
            CambiarEscena();
        }
    }

    private void CambiarEscena()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("PantallaPrincipal");
    }

    public void CompletarObjetivos()
    {
        objetivosCompletados = true;
        if (panelCompleto != null)
        {
            panelCompleto.SetActive(true);
        }
        Debug.Log("Panel de cambio de escena activado");
    }
}