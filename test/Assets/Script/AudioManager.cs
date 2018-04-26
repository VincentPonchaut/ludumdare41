using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource bgmSource;
    public float BgmVolume = 0.5f;

    private AudioSource soundEffectSource;
    public float SoundEffectVolume = 0.5f;

    // Effects
    public AudioClip CorrectAnswerSound;
    public AudioClip WrongAnswerSound;

    #region Methods
    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (bgmSource == null)
            return;

        bgmSource.clip = clip;
        bgmSource.volume = BgmVolume;
        bgmSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectSource.clip = clip;
        soundEffectSource.volume = SoundEffectVolume;
        soundEffectSource.Play();
    }

    public void PlayCorrectAnswerSound()
    {
        PlaySoundEffect(CorrectAnswerSound);
    }

    public void PlayWrongAnswerSound()
    {
        PlaySoundEffect(WrongAnswerSound);
    }
    #endregion

    #region Unity Methods
    // Use this for initialization
    void Start()
    {
        bgmSource = GetComponents<AudioSource>()[0];
        bgmSource.loop = true;

        soundEffectSource = GetComponents<AudioSource>()[1];
        soundEffectSource.loop = false;
    }
    #endregion
}
