using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShowAdvertisement : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private string gameID = "2017a815-ff4e-4b43-bcda-004f48ae2ca0";
    [SerializeField] private string rewardedPlacementID = "";

    private void Awake()
    {
        Advertisement.Initialize(gameID, true);
    }

    private void OnEnable()
    {
        //1. add this script as a listener to the advertisement system
        Advertisement.AddListener(this);
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void ShowRewardedAD()
    {
        if (Advertisement.IsReady())
        {
            //2. Call show slightly differently for a rewared ad 
            // must include a placement id
            Advertisement.Show(rewardedPlacementID);
        }
    }

    //3 write a function to be called when the ad is fully watched
    public void OnUnityAdsDidFinish(string placementID, ShowResult showResult)
    {
        Debug.Log("An ad using placement ID " + placementID + " was shown, result is " + showResult);

        if (placementID == rewardedPlacementID)
        {
            //give reward here
        }

    }

    //4 write a function to be called when the ad is ready
    public void OnUnityAdsReady(string placementID)
    {
        //Debug.Log("An ad using placement ID " + placementID + " is ready");
    }

    //5 write a function to be called when there is an error with ads
    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("An ad experienced an error, message: " + message);
    }

    //6 write a function to be called when an ad starts
    public void OnUnityAdsDidStart(string placementID)
    {
        //Debug.Log("An ad using placement ID " + placementID + " started);
    }

    private void OnDisable()
    {
        //7 remove the listener when the script is destroyed
        Advertisement.RemoveListener(this);
    }
}
