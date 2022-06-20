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

    public GameObject[] waveObjects;
    public Material[] noiseWaveMaterials = new Material[2];



    void Start()
    {
        for (int i = 0; i < waveObjects.Length; i++)
        {
            noiseWaveMaterials[i] = waveObjects[i].GetComponent<Renderer>().material;
        }
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
        ToggleSound(false);
        ToggleWave();
        SetWaveOpacity(0.3f);
        // make the wave easy to see on the creature

    }
    public void ReceiveCompleted()
    {
        SetWaveOpacity(1f);
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
        if (isListen1Completed)
        {
            if (audioSourceNormal.isPlaying)
            {
                audioSourceNormal.DOFade(0, 0.5f);
                audioSourceDistorted.DOFade(0, 0.5f);
            };
            return;
        };

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

    private void ToggleWave()
    {
        foreach (var mat in noiseWaveMaterials)
        {
            mat.DOFloat(1, "_uProgress", 1f);
        }
    }
    private void SetWaveOpacity(float alpha)
    {
        foreach (var mat in noiseWaveMaterials)
        {
            mat.DOFloat(alpha, "_uWaveOpacity", 1f);
        }
    }

}
