using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHolderPosition : MonoBehaviour
{
    public Transform PosicionCamara;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        transform.position = PosicionCamara.position;
    }
}
