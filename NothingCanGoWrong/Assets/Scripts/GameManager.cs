using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            Application.targetFrameRate = 60;
            instance = this;
        }
        else
        {
            return;
        }
    }
    #endregion

    public Animator transition;
    public Animator lightTransition;

    // Update is called once per frame
    void Update()
    {
        if (StarSystem.instance != null)
        {
            if (StarSystem.instance.planetsDestroyed == 3)
            {
                StartCoroutine(LoadLevel(2));
            }
        }
    }

    public void ReturnToMenu()
    {
        Debug.Log("ReturnToMenu called");
        StartCoroutine(LoadLevel(0));
    }
    public void NewGame()
    {
        Debug.Log("NewGame called");
        StartCoroutine(LightTransition());
        StartCoroutine(LoadLevel(1));
    }

    public IEnumerator LoadLevel(int level)
    {
        Debug.Log("LoadLevel" + level + " called");
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(level);
    }

    IEnumerator LightTransition()
    {
        lightTransition.SetTrigger("End");
        yield return new WaitForSeconds(1f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}