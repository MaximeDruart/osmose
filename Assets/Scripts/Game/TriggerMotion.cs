using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class TriggerMotion : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    public GameObject Filter;
    public UnityEvent OnFinishMotion;
    public BoolVariable MotionHasPlayed;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        MotionHasPlayed.Value = false;

        audioSource.volume = 0;
    }

    public void TriggerAnim()
    {
        // play motion
        // then hide it
        audioSource.volume = 1;
        videoPlayer.Play();
        videoPlayer.loopPointReached += (VideoPlayer vp) =>
        {
            Filter.GetComponent<Renderer>().material.DOFade(0f, 0.6f);
            gameObject.SetActive(false);
            OnFinishMotion.Invoke();
            MotionHasPlayed.Value = true;
        };



    }
}