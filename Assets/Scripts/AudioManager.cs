using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }
    AudioSource mainAudioSource;
    public AudioSource sfxAudioSource;
    public AudioSource bgmAudioSource;
    public string audioClipsPath="Audio/";
    private void Start() {
        mainAudioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(string clipName)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioClipsPath + clipName);
        if (audioClip != null)
        {
            Debug.Log("Playing sound " + clipName);
            mainAudioSource.clip = audioClip;

                mainAudioSource.Play();
            //mainAudioSource.PlayOneShot(audioClip);
        }
            
    }
    public void PlaySFX(string clipName)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioClipsPath + clipName);
        if (audioClip != null)
            sfxAudioSource.PlayOneShot(audioClip);
    }
    public void ChangeBGM(string bgmName)
    {
        if (bgmName != null)
        {
            
            AudioClip audioClip = Resources.Load<AudioClip>(audioClipsPath + bgmName);
            if (audioClip != null)
            {
                Debug.LogError("Changing BGM to " + bgmName);
                bgmAudioSource.clip = audioClip;

                bgmAudioSource.Play();
            }

        }

    }
    public void StopSound(){
        mainAudioSource.Stop();
    }
}
