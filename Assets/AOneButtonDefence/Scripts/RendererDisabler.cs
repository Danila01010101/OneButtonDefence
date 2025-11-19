using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererDisabler : IDisposable
{
    private readonly List<Renderer> renderers = new List<Renderer>();
    private Camera mainCamera;
    private bool isActivated = false;
    private float nearDistanceMargin = 8f;
    private float margin = 6f;
    private float renderDistance = 75f;

    private List<Renderer> nearbyRenderers = new List<Renderer>();
    private float updateNearbyInterval = 0.5f;
    private float lastNearbyUpdateTime = 0.3f;

    public IEnumerator Initialize()
    {
        GameInitializer.GameInitialized += StartFindingObjects;
        yield break;
    }

    private void StartFindingObjects() => CoroutineStarter.Instance.StartCoroutine(FindObjects());

    private IEnumerator FindObjects()
    {
        yield return new WaitForSeconds(0.1f);

        mainCamera = Camera.main;

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("RenderDisableable");

        renderers.Clear();
        foreach (var obj in taggedObjects)
        {
            var found = obj.GetComponentsInChildren<Renderer>(true);
            renderers.AddRange(found);
        }

        foreach (var r in renderers)
        {
            if (r != null)
                r.enabled = false;
        }

        isActivated = true;
    }

    public void LateUpdate()
    {
        if (!isActivated || mainCamera == null) return;

        Vector3 camPos = mainCamera.transform.position;

        if (Time.time - lastNearbyUpdateTime > updateNearbyInterval)
        {
            UpdateNearbyRenderers(camPos);
            lastNearbyUpdateTime = Time.time;
        }

        foreach (var r in nearbyRenderers)
        {
            if (r == null) continue;

            bool visible = IsVisible(r);
            r.enabled = visible;
        }

        foreach (var r in renderers)
        {
            if (r == null) continue;
            float distance = Vector3.Distance(camPos, r.transform.position);
            if (distance > renderDistance)
                r.enabled = false;
        }
    }

    private void UpdateNearbyRenderers(Vector3 camPos)
    {
        nearbyRenderers.Clear();

        foreach (var r in renderers)
        {
            if (r == null) continue;
            float distance = Vector3.Distance(camPos, r.transform.position);
            if (distance <= renderDistance)
                nearbyRenderers.Add(r);
        }
    }

    private bool IsVisible(Renderer r)
    {
        Vector3 p = mainCamera.WorldToViewportPoint(r.bounds.center);

        if (p.z < -nearDistanceMargin) return false;

        return p.x >= -margin && p.x <= 1f + margin &&
               p.y >= -margin && p.y <= 1f + margin;
    }

    public void Dispose()
    {
        GameInitializer.GameInitialized -= StartFindingObjects;
    }

    public void SetRenderDistance(float distance) => renderDistance = distance;
}