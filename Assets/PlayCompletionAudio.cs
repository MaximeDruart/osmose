using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCompletionAudio : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource audioSource;
    [SerializeField] private AudioClip EnvironmentClip;
    [SerializeField] private AudioClip ListenClip;
    [SerializeField] private AudioClip MusicClip;
    [SerializeField] private AudioClip DrawingClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayEnvironmentCompleted()
    {
        audioSource.PlayOneShot(EnvironmentClip);
    }
    public void PlayListenCompleted()
    {
        audioSource.PlayOneShot(ListenClip);
    }
    public void PlayMusicCompleted()
    {
        audioSource.PlayOneShot(MusicClip);
    }
    public void PlayDrawingCompleted()
    {
        audioSource.PlayOneShot(DrawingClip);
    }
}
