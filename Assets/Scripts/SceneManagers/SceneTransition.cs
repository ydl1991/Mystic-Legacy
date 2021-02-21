
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    private Animator m_animator;
    public float m_transitionTime = 1f;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void FadeToNextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("NextLevel: " + nextLevel.ToString());
        Debug.Log("CurrentSceneCount: " + SceneManager.sceneCountInBuildSettings.ToString());
        FadeToLevel(nextLevel >= SceneManager.sceneCountInBuildSettings ? 0 : nextLevel);
    }

    public void FadeToLevel(int levelIndex)
    {
        if (levelIndex == 0)
            GameManager.s_instance.ResetGold();

        StartCoroutine(LoadLevel(levelIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        m_animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(m_transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void FadeOut()
    {
        m_animator.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        m_animator.SetTrigger("FadeIn");
    }
}
