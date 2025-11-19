using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererDisabler : IDisposable
{
    private readonly List<Renderer> renderers = new List<Renderer>();
    private readonly List<Renderer> nearbyRenderers = new List<Renderer>();

    private Camera mainCamera;
    private bool isActivated = false;

    private float renderDistance = 75f;
    private float updateNearbyInterval = 0.4f;
    private float lastNearbyUpdateTime = -1f;

    private Plane[] cameraPlanes;

    public IEnumerator Initialize()
    {
        GameInitializer.GameInitialized += StartFindingObjects;
        yield break;
    }

    private void StartFindingObjects()
        => CoroutineStarter.Instance.StartCoroutine(FindObjects());

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
            if (r != null) r.enabled = false;

        isActivated = true;
    }

    public void LateUpdate()
    {
        if (!isActivated || mainCamera == null) return;

        Vector3 camPos = mainCamera.transform.position;
        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        if (Time.time - lastNearbyUpdateTime > updateNearbyInterval)
        {
            UpdateNearbyRenderers(camPos);
            lastNearbyUpdateTime = Time.time;
        }

        foreach (var r in nearbyRenderers)
        {
            if (r == null) continue;
            r.enabled = GeometryUtility.TestPlanesAABB(cameraPlanes, r.bounds);
        }
    }

    private void UpdateNearbyRenderers(Vector3 camPos)
    {
        nearbyRenderers.Clear();

        foreach (var r in renderers)
        {
            if (r == null) continue;

            float d = Vector3.Distance(camPos, r.transform.position);
            if (d <= renderDistance)
                nearbyRenderers.Add(r);
            else
                r.enabled = false;
        }
    }

    public void Dispose()
    {
        GameInitializer.GameInitialized -= StartFindingObjects;
    }

    public void SetRenderDistance(float distance)
        => renderDistance = distance;
}