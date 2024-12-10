using UnityEngine;
using UnityEngine.SceneManagement;

public class Doors : MonoBehaviour
{
    [SerializeField] private string siguienteEscena;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (string.IsNullOrEmpty(siguienteEscena))
            {
                Debug.LogError("Next scene name is not set! Please set it in the Inspector.");
                return;
            }

            SceneManager.LoadScene(siguienteEscena);
        }
    }
}