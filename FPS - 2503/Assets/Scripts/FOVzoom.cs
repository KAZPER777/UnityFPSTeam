using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fovZoom : MonoBehaviour
{
    public Camera playerCamera;
    public float zoomedFOV, normalFOV;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            playerCamera.fieldOfView = zoomedFOV;
        }
        if (Input.GetMouseButtonUp(1))
        {
            playerCamera.fieldOfView = normalFOV;
        }
    }
}