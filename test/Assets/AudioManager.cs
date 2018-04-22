using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource bgmSource;
    public float BgmVolume = 0.5f;

    public void PlayBackgroundMusic(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.volume = BgmVolume;
        bgmSource.Play();
    }

    // Use this for initialization
    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
    }
}
