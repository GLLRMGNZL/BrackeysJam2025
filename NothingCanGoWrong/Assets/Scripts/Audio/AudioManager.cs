using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    //public AudioMixer audioMixer;

    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            // Fill sounds in array with corresponding audio source
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.spatialBlend = 0f;
            s.source.loop = s.loop;
            //s.source.outputAudioMixerGroup = s.group;
            //Debug.Log(s.soundName);
        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Play("menu_music");
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            foreach (Button b in FindObjectsOfType<Button>())
            {
                b.onClick.AddListener(() => Play("button_click"));
            }
        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            foreach (Button b in FindObjectsOfType<Button>())
            {
                b.onClick.AddListener(() => Play("button_click"));
                if (b.name == "PlayButton")
                {
                    b.onClick.AddListener(() => Play("planet_explosion"));
                    b.onClick.AddListener(() => PlayGameMusic());
                }
            }
        }
    }

    public void Play(string name)
    {
        // If the sound we want to play is found in array sounds, play it. If not, go out of the method
        Sound s = Array.Find(sounds, sound => sound.soundName == name);
        if (s == null)
        {
            //Debug.Log("The sound " + name + " was not found!");
            return;
        }
        else
        {
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == name);
        if (s == null)
        {
            //Debug.Log("The sound " + name + " was not found!");
            return;
        }
        else
        {
            s.source.Stop();
        }
    }

    public void PlayGameMusic()
    {
        Stop("menu_music");
        StartCoroutine(PlayGameMusicCoroutine());
    }

    private IEnumerator PlayGameMusicCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Play("game_music");
    }
}
