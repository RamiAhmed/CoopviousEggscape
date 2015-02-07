﻿using UnityEngine;

public class CameraSizeController : MonoBehaviour
{
    public GameObject Player0;
    public GameObject Player1;

    public float startCameraSize = 7f;
    public float minCameraSize = 7f;
    public float maxCameraSize = 15f;

    private Camera _camera;

    // Use this for initialization
    private void Start()
    {
        _camera = this.GetComponent<Camera>();

        _camera.orthographicSize = startCameraSize;
    }

    // Update is called once per frame
    private void Update()
    {
        CenterCamera();
        Resize();
    }

    private void Resize()
    {
        float distance = Vector3.Distance(Player0.transform.position, Player1.transform.position);
        if (distance <= minCameraSize)
        {
            _camera.orthographicSize = minCameraSize;
        }
        else if (distance >= maxCameraSize)
        {
            _camera.orthographicSize = maxCameraSize;
        }
        else
        {
            _camera.orthographicSize = distance;
        }
    }

    private void CenterCamera()
    {
        Vector3 position = CalculateMidVector(Player0.transform.position, Player1.transform.position);

        _camera.transform.position = new Vector3(position.x, _camera.transform.position.y, position.z);
    }

    private Vector3 CalculateMidVector(Vector3 first, Vector3 second)
    {
        return Vector3.Lerp(first, second, 0.5f);
    }
}