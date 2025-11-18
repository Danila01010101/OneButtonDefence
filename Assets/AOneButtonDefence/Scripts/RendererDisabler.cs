using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererDisabler : IDisposable
{
    private readonly List<Renderer> renderers = new List<Renderer>();
    private Camera mainCamera;
    private Plane[] frustumPlanes;
    private bool isActivated = false;
    float margin = 0.2f;
    private float renderDistance = 75f;

    public IEnumerator Initialize()
    {
        GameInitializer.GameInitialized += StartFindingObjects;
        yield break;
    }
    
    private void StartFindingObjects() => CoroutineStarter.Instance.StartCoroutine(FindObjects());

    private IEnumerator FindObjects()
    {
        yield return new WaitForSeconds(3f);
        
        mainCamera = Camera.main;

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("RenderDisableable");

        renderers.Clear();
        foreach (var obj in taggedObjects)
        {
            var found = obj.GetComponentsInChildren<Renderer>(true);
            renderers.AddRange(found);
        }

        isActivated = true;
    }

    public void LateUpdate()
    {
        if (!isActivated) return;

        frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        foreach (var r in renderers)
        {
            bool visible = IsVisible(r, mainCamera);
            r.enabled = visible;
        }
    }

    bool IsVisible(Renderer r, Camera cam)
    {
        Vector3 p = cam.WorldToViewportPoint(r.bounds.center);
        Vector3 camPos = mainCamera.transform.position;
        
        if (p.z < 0f) return false;
        
        float distance = Vector3.Distance(camPos, r.transform.position);
        
        if (distance > renderDistance)
            return(false);

        return p.x >= -margin && p.x <= 1f + margin &&
               p.y >= -margin && p.y <= 1f + margin;
    }

    public void Dispose()
    {
        GameInitializer.GameInitialized -= StartFindingObjects;
    }
}