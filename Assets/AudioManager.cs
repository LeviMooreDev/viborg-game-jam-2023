using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    public enum A
    {
        boing,
        explosion1,
        barrel_break,
        barrel_break2,
        Spikes_up,
        Spikes_down,
        slam1,
        d
    }

    public AudioSource audioSource;
    public AudioClip boing;
    public AudioClip explosion1;
    public AudioClip barrel_break;
    public AudioClip barrel_break2;
    public AudioClip Spikes_up;
    public AudioClip Spikes_down;
    public AudioClip slam1;
    public AudioClip d;

    private void Awake()
    {
        I = this;
    }

    public void Play(A a, float v = 1)
    {
        switch (a)
        {
            case A.d:
                audioSource.PlayOneShot(d);
                break;
            case A.boing:
                audioSource.PlayOneShot(boing);
                break;
            case A.explosion1:
                audioSource.PlayOneShot(explosion1, .5f);
                break;
            case A.barrel_break:
                audioSource.PlayOneShot(barrel_break);
                break;
            case A.barrel_break2:
                audioSource.PlayOneShot(barrel_break2);
                break;
            case A.Spikes_up:
                audioSource.PlayOneShot(Spikes_up, v);
                break;
            case A.Spikes_down:
                audioSource.PlayOneShot(Spikes_down, v);
                break;
            case A.slam1:
                audioSource.PlayOneShot(slam1, v);
                break;
        }
    }
}
