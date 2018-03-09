using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class MARCoreMessages : MonoBehaviour
{
    /// <summary>
    /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
    /// </summary>
    public GameObject searchingForPlaneUI;
    /// <summary>
    /// Should we display the "searching for planes" snackbar?
    /// </summary>
    public bool displaySearchingForPlaneUI = true;
    private bool didSubscribeForSearchingPlaneUI = false;

    private void Start()
    {
        if (displaySearchingForPlaneUI && searchingForPlaneUI != null)
        {
            searchingForPlaneUI.SetActive(false);
            MARCoreEvents.GainedTracking += OnGainedTrackingHideSearchingUI;
            MARCoreEvents.LostTracking += OnLostTrackingShowSearchingUI;
            didSubscribeForSearchingPlaneUI = true;
        }
    }
    private void OnDestroy()
    {
        if (didSubscribeForSearchingPlaneUI)
        {
            MARCoreEvents.GainedTracking -= OnGainedTrackingHideSearchingUI;
            MARCoreEvents.LostTracking -= OnLostTrackingShowSearchingUI;
        }
    }
    private void OnLostTrackingShowSearchingUI()
    {
        searchingForPlaneUI.SetActive(true);
    }

    private void OnGainedTrackingHideSearchingUI()
    {
        searchingForPlaneUI.SetActive(false);
    }
}
