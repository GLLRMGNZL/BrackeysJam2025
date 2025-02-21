using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Credits : MonoBehaviour
{
    public RectTransform contentPanel;
    public float scrollSpeed = 5f;
    public float duration = 25f;
    public RawImage videoScreen;
    public VideoPlayer videoPlayer;

    private Vector2 start;
    private Vector2 end;

    // Start is called before the first frame update
    void Start()
    {
        videoScreen.gameObject.SetActive(false);
        AudioManager.instance.Play("gameover_music");

        start = contentPanel.anchoredPosition;
        start.y = -contentPanel.rect.height + 75f;

        end = contentPanel.anchoredPosition;
        end.y = 180;

        StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time < duration)
        {
            float t = (Time.time - startTime) / duration;
            contentPanel.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }
        Debug.Log("end credits");
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