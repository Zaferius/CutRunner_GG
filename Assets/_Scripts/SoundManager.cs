using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip perfectTap;
    public AudioClip sad;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip,float volume = 0.5f)
    {
        if (clip != null)
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(soundObject, clip.length);
        }
        else
        {
            Debug.LogWarning("Clip 404");
        }
    }
    
    public void PlaySoundRandomPitch(AudioClip clip,float minPitch, float maxPitch,float volume = 0.5f)
    {
        if (clip != null)
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(soundObject, clip.length);
        }
        else
        {
            Debug.LogWarning("Clip 404");
        }
    }

    public void PlaySoundPerfectTap(AudioClip clip, float volume = 0.75f)
    {
        if (clip != null)
        {
            GameObject soundObject = new GameObject("Sound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.pitch = 1;
            audioSource.pitch += GameManager.i.perfectTapCombo / 10f;
            audioSource.clip = perfectTap;
            audioSource.Play();
            Destroy(soundObject, clip.length);
        }
        else
        {
            Debug.LogWarning("Clip 404");
        }
    }
}
