using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource characterAudioSource;
    [SerializeField] private AudioSource zombieAAudioSource;
    [SerializeField] private AudioSource zombieBAudioSource;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlayerMelee(AudioClip sound)
    {
        characterAudioSource.PlayOneShot(sound);
    }

    public void PlayerMoving(AudioClip sound)
    {
        characterAudioSource.PlayOneShot(sound);
    }

    public void StopPlayerMoving()
    {
        characterAudioSource.Stop();
    }

    public void PlayerHitted(AudioClip sound)
    {
        characterAudioSource.PlayOneShot(sound);
    }

    public void PlayZombieIdle(AudioClip sound)
    {
        zombieAAudioSource.PlayOneShot(sound);
    }

    public void PlayZombieHit(AudioClip sound)
    {
        zombieAAudioSource.PlayOneShot(sound);
    }

    internal void ZombieWalking(AudioClip zombieWalkSound)
    {
        this.zombieAAudioSource.PlayOneShot(zombieWalkSound);
    }

    internal void ZombieIdle(AudioClip zombieIdleSound)
    {
        this.zombieAAudioSource.PlayOneShot(zombieIdleSound);
    }

    internal void StopZombieMoving()
    {
        this.zombieAAudioSource.Stop();
    }
}
