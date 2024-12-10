using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CambiarUltimaEcena : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {

            CambiarEscena("PantallaPrincipal");
        }
    }
    private void CambiarEscena(string nombreEscena)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(nombreEscena);
    }


    
}
