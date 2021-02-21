using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneTransition m_transition;

    // Start is called before the first frame update
    void Start()
    {
        //start the level in landscape modes
        Screen.orientation = ScreenOrientation.LandscapeLeft; //setting screen orientation
    }

    public void StartSandbox()
    {
        SceneManager.LoadScene("SandboxLevel");
    }

    public void StartTutorial()
    {
        //load the tutorial level
        m_transition.FadeToLevel(1);
    }

    public void StartGame()
    {
        //load the first level
        m_transition.FadeToLevel(3);
    }
    public void LoadAchievementsScreen()
    {
        //load the achievements screen
        m_transition.FadeToLevel(2);
    }

    public void LoadStore()
    {
        //load the first level
        m_transition.FadeToLevel(9);
    }

    public void ReturnToMainMenu()
    {
        //load the main menu screen
        m_transition.FadeToLevel(0);
    }

    //debug: returning to main menu without transition
    //using only to figure out why it's mad when transitioning to main menu from achievements/store
    public void DebugMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void LoadLevelTwo()
    {
        //load the first level
        m_transition.FadeToLevel(5);
    }

    public void LoadLevelThree()
    {
        //load the first level
        m_transition.FadeToLevel(7);
    }

    
}
