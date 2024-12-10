using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private float maxLookAtDistance = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    private Transform ubi_obj_a_mirar;
    private Vector3 posicionInicial;

    private void Start()
    {
        ubi_obj_a_mirar = GameObject.FindWithTag("Player").transform;
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (ubi_obj_a_mirar != null)
        {
            float distance = Vector3.Distance(transform.position, ubi_obj_a_mirar.position);
            if (distance <= maxLookAtDistance)
            {
                // Calcular direcci�n al jugador
                Vector3 direction = ubi_obj_a_mirar.position - transform.position;
                direction.y = 0; // Ignorar diferencia en altura

                // Solo si hay una direcci�n v�lida
                if (direction != Vector3.zero)
                {
                    // Crear la rotaci�n solo en el eje Y
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    // Mantener la rotaci�n actual en X y Z
                    targetRotation.x = 0;
                    targetRotation.z = 0;

                    // Aplicar la rotaci�n suavemente
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        Time.deltaTime * rotationSpeed
                    );
                }
            }
        }
    }

}
