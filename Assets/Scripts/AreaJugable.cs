using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaJugable : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // Visualizar el área jugable en el editor
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        if (GetComponent<Collider>())
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
}