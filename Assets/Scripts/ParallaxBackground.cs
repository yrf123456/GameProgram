using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;
    private Vector3 lastCameraPos;

    void Start()
    {
        lastCameraPos = cameraTransform.position;
    }

    void Update()
    {
        Vector3 delta = cameraTransform.position - lastCameraPos;
        transform.position += delta * parallaxFactor;
        lastCameraPos = cameraTransform.position;
    }
}
