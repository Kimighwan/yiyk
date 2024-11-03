using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource source;

    public void SetBGMVolume(float volume)
    {
        source.volume = volume;
    }
}
