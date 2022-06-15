using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCompletionAudio : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource audioSource;
    private CompletionState completionState;

    [SerializeField] private AudioClip[] AudioClips;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayMusicOnCompletion()
    {
        audioSource.PlayOneShot(AudioClips[completionState.getNoOfCompletedModules() - 1]);
    }
}
