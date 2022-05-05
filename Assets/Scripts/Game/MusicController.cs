using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource[] audioSources;
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void playSound1(bool isTrue)
    {
        if (isTrue)
        {
            audioSources[0].Play();
        }
    }
    public void playSound2(bool isTrue)
    {
        if (isTrue)
        {
            audioSources[1].Play();
        }
    }
    public void playSound3(bool isTrue)
    {
        if (isTrue)
        {
            audioSources[2].Play();
        }
    }
}
