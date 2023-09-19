using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private float soundEffectVolume;
    [SerializeField] private float soundEffectPitchVariance;
    [SerializeField] private float musicVolume;
    private ObjectPool objectPool;

    private AudioSource musicAudioSource;
    private AudioClip musicClip;

    private void Awake()
    {
        Instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;

        objectPool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        ChangeBackGroundMusic(musicClip);
    }

    public static void ChangeBackGroundMusic(AudioClip musicClip)
    {
        Instance.musicAudioSource.Stop();
        Instance.musicAudioSource.clip = musicClip;
        Instance.musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = Instance.objectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<AudioSource>();
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    }
}
