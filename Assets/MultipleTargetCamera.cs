using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;

    GameObject bouboule;

    public Vector3 offset;
    public float smoothTime = .5f;

    public float speedTimeZoom;
    public float minFOVZoom = 60f;
    public float maxFOVZoom = 30f;
    public float maxOffSet = 100f;
    public float zoomLimiter = 50f;
    public float zoomLimiterOffset = 50f;

    public float maxFOV = 60f;

    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        bouboule = GameObject.Find("Bouboule");
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    void Zoom()
    {

        if (bouboule.GetComponent<Following>().isThrowable)
        {
            cam.fieldOfView = maxFOVZoom;
            return;
        }

        if (cam.fieldOfView < maxFOV)
        {
            float newZoom = Mathf.Lerp(maxFOVZoom, minFOVZoom, GetGreatestDistance() / zoomLimiter);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime * speedTimeZoom);
        }
        else
        {
            float minOffset = offset.z;
            float newZoomOffset = Mathf.Lerp(-maxOffSet, minOffset, GetGreatestDistance() / zoomLimiterOffset);
            offset.z = Mathf.Lerp(offset.z, newZoomOffset, Time.deltaTime);
        }

    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }


    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1 || bouboule.GetComponent<Following>().isThrowable)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
