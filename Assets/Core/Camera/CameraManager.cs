using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float fastMoveMultiplier = 2f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 1f;
    public float maxZoom = 50;

    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;

    private Transform cam; // The actual camera
    private float targetZoom;

    void Start()
    {
        cam = Camera.main.transform;
        targetZoom = cam.localPosition.y;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? fastMoveMultiplier : 1f);
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) direction += transform.forward;
        if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
        if (Input.GetKey(KeyCode.A)) direction -= transform.right;
        if (Input.GetKey(KeyCode.D)) direction += transform.right;

        direction.y = 0; // Keep movement flat
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        cam.localPosition = new Vector3(cam.localPosition.x, Mathf.Lerp(cam.localPosition.y, targetZoom, Time.deltaTime * 10f), cam.localPosition.z);
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
