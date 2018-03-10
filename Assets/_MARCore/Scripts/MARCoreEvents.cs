using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleARCore;

public class MARCoreEvents : MonoBehaviour
{
    private static MARCoreEvents _instance = null;
    public static MARCoreEvents Instance { get { return _instance; } }

    public static event Action GainedTracking;
    public static event Action LostTracking;
    public static event Action<List<TrackedPlane>> GainedTrackedPlanes;

    public List<TrackedPlane> allPlanes = new List<TrackedPlane>();
    public List<TrackedPlane> newPlanes = new List<TrackedPlane>();

    public bool isTracking = false;

    private int trackingPlaneCount = 0;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    private void Update()
    {
        GetTrackablesAndExecuteTrackingEvents();
    }
    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
    private void GetTrackablesAndExecuteTrackingEvents()
    {
        Session.GetTrackables(allPlanes);
        Session.GetTrackables(newPlanes, TrackableQueryFilter.New);

        int lastTrackingPlaneCount = trackingPlaneCount;
        trackingPlaneCount = 0;
        for (int i = 0; i < allPlanes.Count; ++i)
            if (allPlanes[i].TrackingState == TrackingState.Tracking)
                trackingPlaneCount++;

        if (lastTrackingPlaneCount == 0 && trackingPlaneCount > 0)
        {
            isTracking = true;
            if (GainedTracking != null)
                GainedTracking();
        }
        if (lastTrackingPlaneCount > 0 && trackingPlaneCount == 0)
        {
            isTracking = false;
            if(LostTracking != null)
                LostTracking();
        }
        if (newPlanes.Count > 0 && GainedTrackedPlanes != null)
            GainedTrackedPlanes(newPlanes);
    }
}