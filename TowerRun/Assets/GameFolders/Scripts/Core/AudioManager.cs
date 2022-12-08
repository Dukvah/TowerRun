using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX")] 
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource clickSource;

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip inGameMusic;

    [Header("SFX")] 
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip startClick;


    private AudioClip _currentClip;
    private void OnEnable()
    {
        GameManager.Instance.menuMusic.AddListener(MenuMusic);
        GameManager.Instance.menuMusic.AddListener(InGameMusic);
        GameManager.Instance.buttonClick.AddListener(ButtonClick);
        GameManager.Instance.stopMusic.AddListener(StopMusic);
        GameManager.Instance.openMusic.AddListener(OpenMusic);
    }
    private void Start()
    {
        MenuMusic();
    }

    private void MenuMusic()
    {
        _currentClip = menuMusic;
        
        if (PlayerPrefs.GetInt("Music",1) == 0) return;

        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void InGameMusic()
    {
        _currentClip = inGameMusic;
        
        if (PlayerPrefs.GetInt("Music",1) == 0) return;
        
        StartClick();
        
        musicSource.Stop();
        
        musicSource.clip = inGameMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void StopMusic()
    {
        _currentClip = musicSource.clip;
        musicSource.Stop();
    }
    private void OpenMusic()
    {
        musicSource.clip = _currentClip;
        musicSource.Play();
    }
    
    private void ButtonClick()
    {
        if (PlayerPrefs.GetInt("Voice",1) == 0) return;

        clickSource.clip = buttonClick;
        clickSource.Play();
    }

    private void StartClick()
    {
        if (PlayerPrefs.GetInt("Voice",1) == 0) return;

        clickSource.clip = startClick;
        clickSource.Play();
    }
}
