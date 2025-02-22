using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool pause = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        AudioManager.instance.Stop("game_music");
        AudioManager.instance.Play("menu_music");
        GameManager.instance.ReturnToMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}