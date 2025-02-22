using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Credits : MonoBehaviour
{
    public RectTransform contentPanel;
    public float duration;
    public RawImage videoScreen;
    public VideoPlayer videoPlayer;

    private Vector2 start;
    private Vector2 end;

    private void Start()
    {
        videoScreen.gameObject.SetActive(false);
        AudioManager.instance.Play("gameover_music");

        start = contentPanel.anchoredPosition;
        start.y = -contentPanel.rect.height;

        end = contentPanel.anchoredPosition;
        end.y = 200;

        StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            contentPanel.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null; 
        }

        StartCoroutine(TurnOff());
    }

    private IEnumerator TurnOff()
    {
        videoScreen.gameObject.SetActive(true);
        videoPlayer.Play();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("menu_music");
    }
}