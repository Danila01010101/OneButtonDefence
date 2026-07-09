using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererDisabler : IDisposable
{
    private readonly List<Renderer> renderers = new();

    private Camera mainCamera;
    private bool isActivated;

    private float renderDistance = 75f;
    private float renderDistanceSqr;
    private float updateInterval = 0.4f;
    private float nextUpdateTime;

    public IEnumerator Initialize()
    {
        renderDistanceSqr = renderDistance * renderDistance;

        GameInitializer.GameInitialized += StartFindingObjects;
        yield break;
    }

    private void StartFindingObjects()
    {
        CoroutineStarter.Instance.StartCoroutine(FindObjects());
    }

    private IEnumerator FindObjects()
    {
        yield return new WaitForSeconds(0.1f);

        mainCamera = Camera.main;

        GameObject[] taggedObjects =
            GameObject.FindGameObjectsWithTag("RenderDisableable");

        renderers.Clear();

        foreach (var obj in taggedObjects)
        {
            if (obj == null)
                continue;

            renderers.AddRange(obj.GetComponentsInChildren<Renderer>(true));
        }

        isActivated = true;

        UpdateRenderers();
    }

    public void LateUpdate()
    {
        if (!isActivated || mainCamera == null)
            return;

        if (Time.time < nextUpdateTime)
            return;

        nextUpdateTime = Time.time + updateInterval;

        UpdateRenderers();
    }

    private void UpdateRenderers()
    {
        Vector3 camPos = mainCamera.transform.position;

        for (int i = renderers.Count - 1; i >= 0; i--)
        {
            Renderer r = renderers[i];

            if (r == null)
            {
                renderers.RemoveAt(i);
                continue;
            }

            bool shouldBeEnabled =
                (r.transform.position - camPos).sqrMagnitude <= renderDistanceSqr;

            if (r.enabled != shouldBeEnabled)
                r.enabled = shouldBeEnabled;
        }
    }

    public void SetRenderDistance(float distance)
    {
        renderDistance = distance;
        renderDistanceSqr = distance * distance;
    }

    public void Dispose()
    {
        GameInitializer.GameInitialized -= StartFindingObjects;
    }
}