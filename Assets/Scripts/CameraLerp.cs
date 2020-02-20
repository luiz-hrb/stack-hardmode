using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] Vector3 targetPosition;
    [Range(0f, 1f)]
    [SerializeField] float lerp = 0.5f;
    [SerializeField] float zoomOutDistance = 5f;
    [SerializeField] GameObject foreground;
    private bool zoomOut = false;

    #region Monobehaviour Events

    void Update()
    {
        UpdatePosition();
        UpdateScale();
    }
    #endregion

    public void SetTarget(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }

    public void ZoomOut()
    {
        zoomOut = true;
    }

    private void UpdatePosition()
    {
        if (targetPosition != null)
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerp);
    }

    private void UpdateScale()
    {
        if (zoomOut)
        {
            float newScale = Mathf.SmoothStep(transform.localScale.x, zoomOutDistance, lerp);
            transform.localScale = Vector3.one * newScale;
            foreground.SetActive(false);
        }
    }
}
