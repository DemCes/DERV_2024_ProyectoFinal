using UnityEngine;

public class PlayerSafetyManager : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public void StoreOriginalTransform()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void RestoreOriginalTransform()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}