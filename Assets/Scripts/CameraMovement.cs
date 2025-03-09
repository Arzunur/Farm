using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 10f; 
    [SerializeField] public float zoomSpeed = 10f; 
    [SerializeField] public float rotateSpeed = 10f; 
    [SerializeField] public float minZoom = 10f; 
    [SerializeField] public float maxZoom = 20f; 
  
    [SerializeField] public string lookPointLayer = "terrain_main_plane"; 

    [SerializeField] public GameObject redBlockPrefab; 

    private Camera mainCamera; 
    private Vector3 lookPoint;
    private float initialZoom; 
    private Vector2 touchStartPos;
    private float initialPinchDistance;


    private void Awake()
    {
        mainCamera = Camera.main;
        lookPoint = Vector3.zero;
        initialZoom = Mathf.Lerp(minZoom, maxZoom, 0.5f);
        SetZoom(initialZoom);
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.position - touchStartPos;

                Vector3 right = mainCamera.transform.TransformDirection(Vector3.right);
                right.y = 0f;
                right = right.normalized;

                Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
                forward.y = 0f;
                forward = forward.normalized;

                transform.position += right * -touchDelta.x * moveSpeed * Time.deltaTime * 0.01f;
                transform.position += forward * -touchDelta.y * moveSpeed * Time.deltaTime * 0.01f;

                touchStartPos = touch.position;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
            {
                initialPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
            }
            else if (touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved)
            {
                float currentPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                float pinchDelta = currentPinchDistance - initialPinchDistance;

                Vector3 direction = (transform.position - lookPoint).normalized;
                float distance = Vector3.Distance(transform.position, lookPoint);
                float zoomAmount = -pinchDelta * zoomSpeed * Time.deltaTime * 0.1f;
                float newDistance = Mathf.Clamp(distance + zoomAmount, minZoom, maxZoom);
                SetZoom(newDistance);

                initialPinchDistance = currentPinchDistance;
            }
        }
        else if (Input.touchCount == 3)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                UpdateLookPoint();
                float rotateAmount = -touch.deltaPosition.x * rotateSpeed * Time.deltaTime * 0.1f;
                transform.RotateAround(lookPoint, Vector3.up, rotateAmount);
            }
        }
    }

    private void UpdateLookPoint()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask(lookPointLayer)))
        {
            lookPoint = hit.point;
        }
    }

    private void SetZoom(float distance)
    {
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = distance;
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(minZoom, maxZoom, (distance - minZoom) / (maxZoom - minZoom));
        }

        Vector3 direction = (transform.position - lookPoint).normalized;
        transform.position = lookPoint + direction * distance;
    }
}