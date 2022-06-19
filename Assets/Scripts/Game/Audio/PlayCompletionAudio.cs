using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCompletionAudio : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource audioSource;
    public CompletionState completionState;

    [SerializeField] private AudioClip[] AudioClips;


    [Header("Debugging")]

    [SerializeField] private bool isActivated = true;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayMusicOnCompletion()
    {
        if (!isActivated) return;
        audioSource.PlayOneShot(AudioClips[completionState.getNoOfCompletedModules() - 1]);
    }
}
