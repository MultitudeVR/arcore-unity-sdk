using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class MARCoreSpawnPrefabOnTap : MonoBehaviour
{
    public Camera firstPersonCamera;
    public GameObject spawnablePrefab;

    private void Update()
    {
        // If the player has not touched the screen, we are done with this update.
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            var prefab = Instantiate(spawnablePrefab, hit.Pose.position, hit.Pose.rotation);

            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
            // world evolves.
            var anchor = hit.Trackable.CreateAnchor(hit.Pose);

            // prefab should look at the camera but still be flush with the plane.
            if ((hit.Flags & TrackableHitFlags.PlaneWithinPolygon) != TrackableHitFlags.None)
            {
                // Get the camera position and match the y-component with the hit position.
                Vector3 cameraPositionSameY = firstPersonCamera.transform.position;
                cameraPositionSameY.y = hit.Pose.position.y;

                // Have Andy look toward the camera respecting his "up" perspective, which may be from ceiling.
                prefab.transform.LookAt(cameraPositionSameY, prefab.transform.up);
            }

            // Make spawned prefab a child of the anchor.
            prefab.transform.parent = anchor.transform;
        }
    }
}
