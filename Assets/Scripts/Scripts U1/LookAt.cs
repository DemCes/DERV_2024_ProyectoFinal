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
                // Calcular dirección al jugador
                Vector3 direction = ubi_obj_a_mirar.position - transform.position;
                direction.y = 0; // Ignorar diferencia en altura

                // Solo si hay una dirección válida
                if (direction != Vector3.zero)
                {
                    // Crear la rotación solo en el eje Y
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    // Mantener la rotación actual en X y Z
                    targetRotation.x = 0;
                    targetRotation.z = 0;

                    // Aplicar la rotación suavemente
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
