using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.HelloAR;

public class MARCorePlaneVisualizer : MonoBehaviour
{
    /// <summary>
    /// A prefab for tracking and visualizing detected planes.
    /// </summary>
    public GameObject TrackedPlanePrefab;

    private void Start()
    {
        MARCoreEvents.GainedTrackedPlanes += ARCoreEvents_GainedTrackedPlanes;
    }
    private void OnDestroy()
    {
        MARCoreEvents.GainedTrackedPlanes -= ARCoreEvents_GainedTrackedPlanes;
    }
    private void ARCoreEvents_GainedTrackedPlanes(List<TrackedPlane> newPlanes)
    {
        for (int i = 0; i < newPlanes.Count; i++)
        {
            // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
            // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
            // coordinates.
            GameObject planeObject = Instantiate(TrackedPlanePrefab, Vector3.zero, Quaternion.identity,
                transform);
            planeObject.GetComponent<TrackedPlaneVisualizer>().Initialize(newPlanes[i]);
        }
    }
}
