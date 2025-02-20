using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator mainMenuAnim;
    public Animator tutorialAnim;

    public void StartGame()
    {
        GameManager.instance.NewGame();
    }

    public void OpenTutorial()
    {
        mainMenuAnim.SetBool("isOpen", false);
        tutorialAnim.SetBool("isOpen", true);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
