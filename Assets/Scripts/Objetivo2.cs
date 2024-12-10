using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Objetivo2 : MonoBehaviour
{
    [Header("Referencias de Objetos")]
    [SerializeField] private MeshRenderer[] objectsToReveal;
    [SerializeField] private float revealTime = 5f;
    [SerializeField] private float interactionDistance = 2f;

    [Header("UI")]
    [SerializeField] private Canvas objetivoCanvas;
    [SerializeField] private GameObject panelCompletado;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI completionText;

    [Header("Cambio de Escena")]
    [SerializeField] private CambioEscena panelCambioEscena;

    private bool hasTape = false;
    private bool isRevealing = false;
    private float currentRevealTime;
    private Transform player;
    private int revealedObjects = 0;
    private MeshRenderer currentTargetObject;

    void Start()
    {
        ValidarConfiguracion();
        ConfigurarUI();
        ConfigurarObjetosOcultos();


        if (panelCompletado != null)
        {
            panelCompletado.SetActive(false);
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("No se encontró un objeto con tag 'Player' en la escena");
        }
    }

    private void ValidarConfiguracion()
    {
        if (objectsToReveal == null || objectsToReveal.Length == 0)
            Debug.LogError("No hay objetos para revelar asignados");

        if (countdownText == null)
            Debug.LogError("Falta asignar el texto de countdown");
    }

    private void ConfigurarUI()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void ConfigurarObjetosOcultos()
    {
        foreach (var obj in objectsToReveal)
        {
            if (obj != null)
            {
                obj.enabled = false;
                Debug.Log($"Objeto {obj.gameObject.name} configurado como invisible");
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (revealedObjects >= objectsToReveal.Length)
        {
            ShowCompletionMessage();
            return;
        }

        CheckNearbyObjects();
    }

    private void CheckNearbyObjects()
    {
        if (!hasTape) return;

        foreach (var obj in objectsToReveal)
        {
            if (obj == null || obj.enabled) continue;

            Vector3 playerPos = new Vector3(player.position.x, obj.transform.position.y, player.position.z);
            Vector3 objPos = obj.transform.position;
            float distanceToObject = Vector3.Distance(objPos, playerPos);

            if (distanceToObject <= interactionDistance)
            {
                currentTargetObject = obj;

                if (Input.GetKey(KeyCode.E))
                {
                    if (!isRevealing)
                    {
                        StartRevealProcess();
                    }
                    ContinueRevealProcess();
                }
                else if (isRevealing)
                {
                    CancelRevealProcess();
                }
                break;
            }
        }
    }

    private void StartRevealProcess()
    {
        isRevealing = true;
        currentRevealTime = revealTime;
        countdownText.gameObject.SetActive(true);
    }

    private void ContinueRevealProcess()
    {
        currentRevealTime -= Time.deltaTime;
        countdownText.text = $"Mantenga E: {Mathf.Ceil(currentRevealTime)}";

        if (currentRevealTime <= 0)
        {
            RevealObject();
        }
    }

    private void CancelRevealProcess()
    {
        isRevealing = false;
        countdownText.gameObject.SetActive(false);
    }

    private void RevealObject()
    {
        if (currentTargetObject != null)
        {
            currentTargetObject.enabled = true;
            revealedObjects++;
            Debug.Log($"Objeto revelado. Objetos restantes: {objectsToReveal.Length - revealedObjects}");
        }

        isRevealing = false;
        countdownText.gameObject.SetActive(false);
        hasTape = false;
        TomarCinta.ResetTapeStatus();
    }

    private void ShowCompletionMessage()
    {
        if (completionText != null)
        {
            completionText.text = "Tapa las ventanas con la madera contrachapada (completado)";
            completionText.gameObject.SetActive(true);
            Debug.Log("¡Objetivo 2 completado!");
            StartCoroutine(MostrarPanelCompletado());

            // Verificar si Objetivo1 está completado
            var objetivo1 = FindObjectOfType<Objetivo1>();
            if (objetivo1 != null && objetivo1.EstanSacosColocados())
            {
                if (panelCambioEscena != null)
                {
                    panelCambioEscena.CompletarObjetivos();
                }
            }
        }
    }

    private IEnumerator MostrarPanelCompletado()
    {
        if (panelCompletado != null)
        {
            panelCompletado.SetActive(true);
            yield return new WaitForSeconds(10f);
            Destroy(panelCompletado);
        }
    }

    public void CollectTape()
    {
        Debug.Log("Cinta recogida");
        hasTape = true;
    }

    public bool EstanTodosObjetosRevelados()
    {
        return revealedObjects >= objectsToReveal.Length;
    }
}