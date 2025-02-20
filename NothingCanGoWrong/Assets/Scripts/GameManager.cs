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
            instance = this;
        }
        else
        {
            return;
        }
    }
    #endregion

    public Animator transition;

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
        StartCoroutine(LoadLevel(1));
    }

    IEnumerator LoadLevel(int level)
    {
        Debug.Log("LoadLevel" + level + " called");
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}