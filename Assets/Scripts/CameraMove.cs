using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    private const string MouseScrollWheelAxis = "Mouse ScrollWheel";

    public Camera MainCamera;

    private Vector3 DragOrigin;
    private float cameraZoomStep = 2;
    private float minCameraZoom = 5;

    void OnAwake()
    {
        MainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        CheckForZoom();
        CheckForMove();
    }

    private void CheckForZoom()
    {
        var mouseScrollWheelAxisValue = Input.GetAxis(MouseScrollWheelAxis);
        if (mouseScrollWheelAxisValue != 0)
        {
            var futureOrthographicCameraSize = MainCamera.orthographicSize + (cameraZoomStep * (mouseScrollWheelAxisValue * - 1));
            if (futureOrthographicCameraSize < minCameraZoom)
                return;
            MainCamera.orthographicSize = futureOrthographicCameraSize;
        }

    }

    private void CheckForMove()
    {
        if(Input.GetMouseButtonDown(1))
            DragOrigin = MainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(1))
        {
            var difference = DragOrigin - MainCamera.ScreenToWorldPoint(Input.mousePosition);
            MainCamera.transform.position += difference;
        }
    }
}
