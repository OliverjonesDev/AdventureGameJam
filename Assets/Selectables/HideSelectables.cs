using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSelectables : MonoBehaviour
{
    [SerializeField]
    private float distanceMax = 1500;
    // Update is called once per frame
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector3.Distance(worldPosition, transform.position);
        if (distance <= distanceMax)
        {
            GetComponent<ParticleSystem>().enableEmission = true;
        }
        else
        {
            GetComponent<ParticleSystem>().enableEmission = false;
        }
    }
}
