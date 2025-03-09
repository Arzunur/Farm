using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 touchOffset;

    private bool isDragging = false;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
                {
                    touchOffset = transform.position - GetTouchWorldPosition(touch.position);
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                transform.position = GetTouchWorldPosition(touch.position) + touchOffset;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    private Vector3 GetTouchWorldPosition(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
