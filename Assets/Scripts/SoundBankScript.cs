using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Centralize game sounds
/// </summary>
public class SoundBankScript : MonoBehaviour
{
    public static SoundBankScript Instance;

    public AudioClip Start;
    public AudioClip Die;
    public List<AudioClip> Eat;
    public List<AudioClip> Explosion;
    public List<AudioClip> Jump;

    void Awake()
    {
        Instance = this;
    }

    public void Play(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
