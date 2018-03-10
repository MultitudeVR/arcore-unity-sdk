using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures.TransformGestures;
using GoogleARCore;

/// <summary>
/// Manipulate an ARCore-tracked object with TouchScript. Scale, rotate, and place it anywhere.
/// </summary>
public class MARCoreManipulateObject : MonoBehaviour
{
    public Camera firstPersonCamera;
    public ScreenTransformGesture scaleRotateGesture;
    public ScreenTransformGesture moveGesture;
    public TouchScript.Gestures.TapGesture tapGesture;
    public GameObject trackedObject;
    public bool setTrackedObjectActiveWhenTracked = true;
    public bool placeTrackedObjectWhenGainedTracking = true;

    public float panSpeed = .0004f;
    public float rotationSpeed = 1f;
    public float zoomSpeed = 1f;
    
    private void OnEnable()
    {
        MARCoreEvents.GainedTracking += MARCoreEvents_GainedTracking;
        MARCoreEvents.LostTracking += MARCoreEvents_LostTracking;
        moveGesture.Transformed += MoveTransformHandler;
        scaleRotateGesture.Transformed += ScaleRotateTransformHandler;
        tapGesture.Tapped += TapGesture_Tapped;
    }

    private void OnDisable()
    {
        MARCoreEvents.GainedTracking -= MARCoreEvents_GainedTracking;
        MARCoreEvents.LostTracking -= MARCoreEvents_LostTracking;
        moveGesture.Transformed -= MoveTransformHandler;
        scaleRotateGesture.Transformed -= ScaleRotateTransformHandler;
        tapGesture.Tapped -= TapGesture_Tapped;
    }
    private void MARCoreEvents_LostTracking()
    {
        if(setTrackedObjectActiveWhenTracked)
            trackedObject.SetActive(false);
    }

    private void MARCoreEvents_GainedTracking()
    {
        if (setTrackedObjectActiveWhenTracked)
            trackedObject.SetActive(true);
        if(placeTrackedObjectWhenGainedTracking)
        {
            for(int i = 0; i < MARCoreEvents.Instance.allPlanes.Count; ++i)
                if(MARCoreEvents.Instance.allPlanes[i].TrackingState == GoogleARCore.TrackingState.Tracking)
                {
                    var plane = MARCoreEvents.Instance.allPlanes[i];
                    MoveTrackedObjectToPose(plane, plane.CenterPose);
                    break;
                }
        }
    }

    private void TapGesture_Tapped(object sender, System.EventArgs e)
    {
        Touch touch = Input.GetTouch(0);

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            MoveTrackedObjectToPose(hit.Trackable, hit.Pose);
        }
    }
    private void MoveTrackedObjectToPose(Trackable plane, Pose pose)
    {
        //move the object to the center of tracked plane.
        trackedObject.transform.position = pose.position;
        trackedObject.transform.rotation = pose.rotation;
        //make the object look at the camera.
        Vector3 camPos = firstPersonCamera.transform.position;
        camPos.y = trackedObject.transform.position.y;
        trackedObject.transform.LookAt(camPos);
        //spawn anchor and parent object to it
        var anchor = plane.CreateAnchor(pose);
        trackedObject.transform.parent = anchor.transform;
    }
    private void ScaleRotateTransformHandler(object sender, System.EventArgs e)
    {
        trackedObject.transform.localRotation = Quaternion.AngleAxis(scaleRotateGesture.DeltaRotation, Vector3.down) * trackedObject.transform.localRotation;
        trackedObject.transform.localScale *= scaleRotateGesture.DeltaScale * zoomSpeed;
    }

    private void MoveTransformHandler(object sender, System.EventArgs e)
    {
        Vector3 dir = firstPersonCamera.transform.TransformDirection(moveGesture.DeltaPosition);
        trackedObject.transform.position += Vector3.ProjectOnPlane(dir, Vector3.up).normalized * moveGesture.DeltaPosition.magnitude * panSpeed;
    }
}
