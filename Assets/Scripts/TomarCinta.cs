using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class TomarCinta : MonoBehaviour
    {
        [SerializeField] private float pickupDistance = 2f;

        private Transform player;
        private Objetivo2 objetivo2Script;
        private bool hasBeenCollected = false;
        private MeshRenderer meshRenderer;
        private static bool playerHasTape = false; // Variable estática para controlar si el jugador tiene una cinta

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("TomarCinta: No se encontró objeto con tag 'Player' en la escena");
            }

            objetivo2Script = FindObjectOfType<Objetivo2>();
            if (objetivo2Script == null)
            {
                Debug.LogError("TomarCinta: No se encontró el script Objetivo2 en la escena");
            }

            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (player == null || objetivo2Script == null || hasBeenCollected || playerHasTape) return;

            Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            Vector3 thisPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            float distanceToPlayer = Vector3.Distance(thisPos, playerPos);

            if (distanceToPlayer <= pickupDistance && Input.GetKeyDown(KeyCode.E))
            {
                RecogerCinta();
            }
        }

        private void RecogerCinta()
        {
            if (!playerHasTape)
            {
                hasBeenCollected = true;
                playerHasTape = true;
                meshRenderer.enabled = false;
                objetivo2Script.CollectTape();
            }
        }

        public static void ResetTapeStatus()
        {
            playerHasTape = false;
        }
    }
