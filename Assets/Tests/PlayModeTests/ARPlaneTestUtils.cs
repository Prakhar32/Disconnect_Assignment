using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Test utilities for finding ARPlanes visible to a camera's view frustum.
/// Designed for use in PlayMode / integration tests.
/// </summary>
public static class ARPlaneTestUtils
{
    /// <summary>
    /// Finds the ARPlane that is most inside the camera frustum. If an ARPlaneManager is provided it
    /// will use its trackables; otherwise falls back to FindObjectsOfType&lt;ARPlane&gt;().
    /// Returns null if none found.
    /// </summary>
    public static ARPlane FindFirstPlaneInFrustum(
        ARPlaneManager planeManager = null,
        Camera camera = null,
        float viewportMargin = 0f,
        bool samplePlaneExtents = true)
    {
        return FindBestPlaneInFrustum(planeManager, null, camera, viewportMargin, samplePlaneExtents);
    }

    /// <summary>
    /// Finds the ARPlane that is most inside the camera frustum excluding the supplied <paramref name="excludePlane"/>.
    /// The excluded plane is ignored in the search; pass null to exclude none.
    /// Returns null if none found.
    /// </summary>
    public static ARPlane FindFirstPlaneInFrustum(
        ARPlaneManager planeManager,
        ARPlane excludePlane,
        Camera camera = null,
        float viewportMargin = 0f,
        bool samplePlaneExtents = true)
    {
        return FindBestPlaneInFrustum(planeManager, excludePlane, camera, viewportMargin, samplePlaneExtents);
    }

    /// <summary>
    /// Internal implementation that scores planes by how many sampled points fall inside the viewport
    /// and returns the plane with the highest normalized score (ties broken by closer distance).
    /// </summary>
    static ARPlane FindBestPlaneInFrustum(
        ARPlaneManager planeManager,
        ARPlane excludePlane,
        Camera camera,
        float viewportMargin,
        bool samplePlaneExtents)
    {
        var cam = camera ?? Camera.main;
        if (cam == null)
            return null;

        ARPlane bestPlane = null;
        float bestScore = 0f;
        float bestDistance = float.MaxValue;

        foreach (var plane in planeManager.trackables)
        {
            if (plane == null || plane == excludePlane)
                continue;

            var (score, distance) = EvaluatePlaneVisibilityScore(plane, cam, viewportMargin, samplePlaneExtents);

            // We require a strictly positive score to consider the plane visible at all.
            if (score <= 0f)
                continue;

            // Prefer higher score; tie-break by closer distance.
            if (score > bestScore || (Mathf.Approximately(score, bestScore) && distance < bestDistance))
            {
                bestScore = score;
                bestPlane = plane;
                bestDistance = distance;
            }
        }

        return bestPlane;
    }

    /// <summary>
    /// Samples points on the plane (center + optional extents/corners) and returns a tuple:
    /// (normalizedVisibilityScore [0..1], distanceFromCamera).
    /// Score = #insideViewport / #samples.
    /// </summary>
    static (float score, float distance) EvaluatePlaneVisibilityScore(ARPlane plane, Camera cam, float viewportMargin, bool sampleExtents)
    {
        if (plane == null || cam == null)
            return (0f, float.MaxValue);

        var samples = new List<Vector3> { plane.transform.position };

        if (sampleExtents)
        {
            // Try to sample edges/corners using ARPlane.size (width=x, height=y)
            // Fallback to center-only if size isn't available for the ARFoundation version.
            try
            {
                var size = plane.size;
                var halfX = size.x * 0.5f;
                var halfZ = size.y * 0.5f;
                var right = plane.transform.right;
                var forward = plane.transform.forward;
                var center = plane.transform.position;

                // edge midpoints
                samples.Add(center + right * halfX);
                samples.Add(center - right * halfX);
                samples.Add(center + forward * halfZ);
                samples.Add(center - forward * halfZ);

                // corners
                samples.Add(center + right * halfX + forward * halfZ);
                samples.Add(center + right * halfX - forward * halfZ);
                samples.Add(center - right * halfX + forward * halfZ);
                samples.Add(center - right * halfX - forward * halfZ);
            }
            catch
            {
                // ignore and use center-only
            }
        }

        var min = -viewportMargin;
        var max = 1f + viewportMargin;
        var insideCount = 0;
        var minDistance = float.MaxValue;

        foreach (var p in samples)
        {
            var vp = cam.WorldToViewportPoint(p);
            // z>0 => in front of camera
            if (vp.z > 0f)
            {
                if (vp.x >= min && vp.x <= max && vp.y >= min && vp.y <= max)
                    insideCount++;
                // track distance to camera for tie-breaking (use distance to center)
                var dist = Vector3.Distance(cam.transform.position, p);
                if (dist < minDistance)
                    minDistance = dist;
            }
            else
            {
                // point behind camera => treat as outside, but still update distance using absolute z
                var dist = Mathf.Abs(vp.z);
                if (dist < minDistance)
                    minDistance = dist;
            }
        }

        var score = samples.Count > 0 ? (float)insideCount / samples.Count : 0f;
        return (score, minDistance);
    }
}