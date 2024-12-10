using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2_CalcularDistancia : MonoBehaviour
{
    [SerializeField] private float maxDistance = 5f;
    private Transform ubi_obj_cal_dist;
    private float distance;

    public float getDistance()
    {
        return distance;
    }

    private void Start()
    {
        ubi_obj_cal_dist = GameObject.Find("Jugador").GetComponent<Transform>();
    }

    void Update()
    {
        if (ubi_obj_cal_dist != null)
        {
            distance = Vector3.Distance(transform.position, ubi_obj_cal_dist.position);
        }
    }
}