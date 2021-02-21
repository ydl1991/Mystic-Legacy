using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

using PlayGamesPlatform = GooglePlayGames.PlayGamesPlatform;

public class GooglePlayGamesManager : MonoBehaviour
{
    private string status = "Not Signed In";
    private static GooglePlayGames.PlayGamesPlatform platform = null;

    public static bool IsSignedIn { get { return platform != null; } }


    private void Awake()
    {
        // Configure the Google Play Games setup
        PlayGamesClientConfiguration googlePlayConfig = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(googlePlayConfig);
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games service
        GooglePlayGames.PlayGamesPlatform.Activate();

        // Use the "CanPromptOnce" flow to try to automatically sign them in
        GooglePlayGames.PlayGamesPlatform.Instance.Authenticate(GooglePlayGames.BasicApi.SignInInteractivity.CanPromptOnce, OnAttemptSilentLogIn);

    }

    private void OnAttemptSilentLogIn(GooglePlayGames.BasicApi.SignInStatus result)
    {
        if (result == GooglePlayGames.BasicApi.SignInStatus.Success)
        {
            OnAttemptAuthentication(true);
            status = "Signed in silently";
        }
        else
        {
            // This will print the value of result as a string
            status = "Silent sign in failed, status was reported as " + System.Enum.GetName(typeof(GooglePlayGames.BasicApi.SignInStatus), result);
        }
    }
    
    
    private void OnGUI()
    {
#if UNITY_EDITOR
        bool isSignedIn = true;
#else
        // authenticated will be true if the user has previously signed in, either manually or through the CanPromptOnce flow
        bool isSignedIn = Social.localUser.authenticated;
#endif
        //GUILayout.Label(status);

        //if (isSignedIn == false)
        //{
        //    if (GUILayout.Button("Sign In"))
        //    {
        //        // Send a signal to the server, attempting to sign in the user.
        //        // The server will respond and its response will be passed into OnAttempAuthentication
        //        Social.localUser.Authenticate(OnAttemptAuthentication);
        //    }
        //}
        //else
        //{
        //    // signed in

        //    if (GUILayout.Button("Sign out"))
        //    {
        //        // Make sure the platform is not null.
        //        // If it is null, then something went wrong.
        //        if (platform == null)
        //        {
        //            status = "Google Play Games didn't load correctly; Social.Active was null";
        //        }
        //        else
        //        {
        //            platform.SignOut();
        //        }
        //    }
        //}


        if(SceneManager.GetActiveScene().name  == "AchievementsScreen")
        {

            if (isSignedIn == false)
            {
                
            }
            else
            {
                
                //if (GUILayout.Button("Rookie"))
                //{
                //    Social.ReportProgress(GooglePlayIds.achievement_rookie, 100.0, OnProgressAchievement);
                //}

                //if (GUILayout.Button("Upgrade Master"))
                //{
                //    Social.ReportProgress(GooglePlayIds.achievement_upgrade_master, 20.0, OnProgressAchievement);
                //}

                //if (GUILayout.Button("Heavy Hitter"))
                //{
                //    Social.ReportProgress(GooglePlayIds.achievement_heavy_hitter, 2.0, OnProgressAchievement);
                //}

                //if (GUILayout.Button("Skilled Attacker"))
                //{
                //    Social.ReportProgress(GooglePlayIds.achievement_skilled_attacker, 6.67, OnProgressAchievement);
                //}

                //if (GUILayout.Button("Honorable Defender"))
                //{
                //    Social.ReportProgress(GooglePlayIds.achievement_honorable_defender, 100.0, OnProgressAchievement);
                //}

            }

        }

    }
    private void OnAttemptAuthentication(bool didSucceed)
    {
        if (didSucceed)
        {
            platform = Social.Active as GooglePlayGames.PlayGamesPlatform;
        }
        else
        {
            status = "Authentication failed";
        }
    }
    public void ShowAchivementsUI()
    {
        Debug.Log("Showing achievement UI");
        Social.ShowAchievementsUI();
    }

    public void GiveGettingStartedAchievement()
    {
        Social.ReportProgress(GooglePlayIds.achievement_getting_started, 100.0, OnProgressAchievement);
        Debug.Log("Gave getting started achievement");
    }


    public void GiveRookieAchivement()
    {
        Social.ReportProgress(GooglePlayIds.achievement_rookie, 100.0, OnProgressAchievement);
        Debug.Log("Gave rookie achievement");
    }

    public void UpdateUpgradeMasterAchivement()
    {
        if (platform == null)
            return;
            
        platform.IncrementAchievement(GooglePlayIds.achievement_upgrade_master, 1, (bool success) => {});
        Debug.Log("Updated upgrade master achievement");

    }

    public void UpdateHeavyHitterAchievement()
    {
        if (platform == null)
            return;

        platform.IncrementAchievement(GooglePlayIds.achievement_heavy_hitter, 1, (bool success) => { });
        Debug.Log("Updated heavy hitter achievement");

    }

    public void UpdateSkilledAttackerAchievement()
    {
        if (platform == null)
            return;

        platform.IncrementAchievement(GooglePlayIds.achievement_skilled_attacker, 1, (bool success) => { });
        Debug.Log("Updated skilled attacker achievement");

    }

    public void UpdateHonorableDefenderAchievement()
    {
        Social.ReportProgress(GooglePlayIds.achievement_honorable_defender, 100.0, OnProgressAchievement);
        Debug.Log("Gave honorable defender achievement");

    }

    private void OnProgressAchievement(bool result)
    {
        status = "Achievement result: " + result;
    }
}
