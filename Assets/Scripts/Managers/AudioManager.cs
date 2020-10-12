using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip _powerupClip;

    private AudioSource _audio;

    void OnEnable()
    {
        Powerup.onPowerupPickup += PlayPowerupSoundClip;
    }

    void Start()
    {
        _audio = GetComponent<AudioSource>();

        if (_audio == null)
        {
            Debug.LogError("Audio Source on the Audio Manager is NULL");
        }
    }

    void PlayPowerupSoundClip()
    {
        _audio.clip = _powerupClip;
        _audio.Play();
    }

    void OnDisable()
    {
        Powerup.onPowerupPickup -= PlayPowerupSoundClip;
    }
}
