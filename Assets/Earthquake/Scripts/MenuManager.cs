using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button terremoto;
    public Button inundacion;
    public Button salir;
    public string escena_terremoto = "Escena1";
    public string Escena_inundacion = "Escena2";

    private void Start()
    {
        if (terremoto != null && inundacion != null && salir != null)
        {
            Debug.Log("Botones asignados correctamente");
            terremoto.onClick.AddListener(OnTerremotoClick);
            inundacion.onClick.AddListener(OnInundacionClick);
            salir.onClick.AddListener(SalirDelJuego);
        }
        else
        {
            Debug.LogError("Faltan botones asignados en el Inspector.");
        }
    }

    public void OnTerremotoClick()
    {
        Debug.Log("Cargando escena de terremoto...");
        SceneManager.LoadScene(escena_terremoto);
    }

    public void OnInundacionClick()
    {
        Debug.Log("Cargando escena de inundación...");
        SceneManager.LoadScene(Escena_inundacion);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
