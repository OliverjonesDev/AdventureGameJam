using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public bool beingDragged;
    private Collider2D collider;
    [SerializeField]
    private Collider2D fire;
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 400;
        if (fire.gameObject.GetComponent<Fire>().timerStarted)
        {
            if (beingDragged)
            {
                gameObject.transform.position = worldPosition;
                if (Input.GetMouseButtonDown(0))
                {
                    beingDragged = false;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (collider.bounds.Contains(worldPosition))
                {
                    beingDragged = true;
                }
                else
                {
                    beingDragged = false;
                }
            }

            if (!beingDragged && fire.bounds.Contains(gameObject.transform.position))
            {
                Debug.Log("Entered");
                fire.GetComponent<Fire>().AddFirewood(gameObject);
            }
        }
    }
}
