using DG.Tweening;
using UnityEngine;

public class ReceiveListen : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] private AudioSource audioSourceNormal;
    [SerializeField] private AudioSource audioSourceDistorted;
    [SerializeField] private float maxDistortionDistance = 3;

    private float volumeMix = 0;

    private bool isListen1Completed = false;



    void Start()
    {

    }


    // ADRESS : /listen/frequency
    public void ReceiveFreq(float distance)
    {

        HandleAudio(distance);
        // adjust the wave opacity between 0 and bototm threshold
        // not sure if we still want it
    }

    // ADRESS : /listen/frequency/completed
    public void ReceiveFreqCompleted()
    {
        isListen1Completed = true;
        // make the wave easy to see on the creature

    }

    // ADRESS : /listen/color
    public void ReceiveColor(Color color)
    {
        // adjust the lights and fog according to this color
    }

    private void HandleAudio(float distance)
    {
        volumeMix = extOSC.OSCUtilities.Map(distance, 0, maxDistortionDistance, 0, 1, true);

        audioSourceNormal.volume = 1 - volumeMix;
        audioSourceDistorted.volume = volumeMix;
    }

    public void ToggleSound(bool isOn)
    {
        if (isListen1Completed) return;

        if (isOn)
        {
            if (!audioSourceNormal.isPlaying)
            {
                audioSourceNormal.Play();
                audioSourceDistorted.Play();
            };

            audioSourceNormal.DOFade(1 - volumeMix, 0.5f);
            audioSourceDistorted.DOFade(volumeMix, 0.5f);

        }
        else
        {
            audioSourceNormal.DOFade(0, 0.5f);
            audioSourceDistorted.DOFade(0, 0.5f);
        }
    }
}
