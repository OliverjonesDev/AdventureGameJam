using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private float smoothness;
    [SerializeField]
    private float horizontalMovementPos, horizontalMovementNeg, verticalMovementPos, verticalMovementNeg;
    private JsonSaving savedData;
    private Camera _camera;
    private GameObject[] particlesInScene;
    private void Awake()
    {
        particlesInScene = GameObject.FindGameObjectsWithTag("SelectParticles");
        savedData = FindObjectOfType<JsonSaving>();
        _camera = Camera.main;
    }
    private void Update()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(Mathf.Lerp(transform.position.x, transform.position.x + Input.GetAxisRaw("Mouse X") *savedData.settings.sensitivity,smoothness * Time.deltaTime), horizontalMovementNeg, horizontalMovementPos);
        pos.y = Mathf.Clamp(Mathf.Lerp(transform.position.y, transform.position.y + Input.GetAxisRaw("Mouse Y") * savedData.settings.sensitivity, smoothness * Time.deltaTime), verticalMovementNeg, verticalMovementPos);
        transform.position = pos;

        if (Input.GetMouseButton(3))
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 1200, Time.deltaTime * smoothness);
        }
        else 
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 1600, Time.deltaTime * smoothness);
        }
    }
}
