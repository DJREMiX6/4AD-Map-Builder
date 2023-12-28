using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    public GameObject mapContainer;
    public float scale = 0.5f;

    private Vector3 DragOrigin;
    private float cameraZoomStep = 2;
    private float minCameraZoom = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForZoom();
        CheckForMove();
    }

    private void CheckForZoom()
    {
        var mouseScrollWheelAxisValue = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheelAxisValue != 0)
        {
            var futureOrthographicCameraSize = Camera.main.orthographicSize + (cameraZoomStep * (mouseScrollWheelAxisValue * - 1));
            if (futureOrthographicCameraSize < minCameraZoom)
                return;
            Camera.main.orthographicSize = futureOrthographicCameraSize;
        }

    }

    private void CheckForMove()
    {
        if(Input.GetMouseButtonDown(1))
            DragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(1))
        {
            var difference = DragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += difference;
        }
    }
}
