using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;

    Camera cam;
    float height;
    float width;

    [Header("Bounds")]
    public float leftBound = 10;
    public float rightBound = 10;
    public float topBound = 10;
    public float bottomBound = 10;

    private void Start()
    {
        cam = GetComponent<Camera>();
        updateCameraDimensions();
    }

    private void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x,-leftBound + width/2, rightBound - width / 2),Mathf.Clamp(target.position.y,-bottomBound + height/2,topBound - height/2),-10);
    }

    void updateCameraDimensions()
    {
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 dimensions = new Vector3(leftBound + rightBound, topBound + bottomBound);
        Vector3 center = -(new Vector3(leftBound, bottomBound)) + (dimensions /2);
        Gizmos.DrawWireCube(center, dimensions);
    }
}
