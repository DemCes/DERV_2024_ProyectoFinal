using UnityEngine;
using System.Collections;

public class MoveObjectController : MonoBehaviour
{
    GameObject thedoor;
    public bool isUnlocked = false;

    void OnTriggerEnter(Collider obj)
    {

        if (isUnlocked)
        {
            thedoor = GameObject.FindWithTag("PFB_Closet");
            thedoor.GetComponent<Animation>().Play("Closet_Open");
        }
    }

    void OnTriggerExit(Collider obj)
    {

        if (isUnlocked)
        {
            thedoor = GameObject.FindWithTag("PFB_Closet");
            thedoor.GetComponent<Animation>().Play("Closet_Close");
        }
    }
}
