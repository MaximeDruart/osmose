using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    private float[] _freqBand = new float[8];
    private float[] _freqBandHighest = new float[8]; 
    private float[] _bandBuffer = new float[8]; 
    private float[] _bufferDecrease = new float[8];
    private float _amplitudeHighest;
    
    public static float[] _samples = new float[512];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];
    public static float _amplitude;
    public static float _amplitudeBuffer;

    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private bool _useMicrophone;
    [SerializeField] private string _selectedDevice;
    
    private void Start()
    {

        _audioSource = GetComponent<AudioSource>();
        SetupMicrophoneOnAudioSource();
        _audioSource.Play();
        // int minFreq, maxFreq = 0;
        // Microphone.GetDeviceCaps(_selectedDevice, out minFreq, out maxFreq);
        // Debug.Log(min);
    }

    private void Update()
    {
        GetSpectrumDataAudioSource();
        MakeFrequencyBand();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }
    
    private void SetupMicrophoneOnAudioSource() {
        bool hasConnectedDevices = Microphone.devices.Length > 0;
        if (_useMicrophone && hasConnectedDevices)
        {
            _selectedDevice = Microphone.devices[0].ToString();
            _audioSource.clip = Microphone.Start(_selectedDevice, true, 10, AudioSettings.outputSampleRate);
        }
        else if (!hasConnectedDevices) _useMicrophone = false;
        else if (!_useMicrophone) Debug.Log("Microphone is not listen");
        else Debug.Log("Error");
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i]) _freqBandHighest[i] = _freqBand[i];
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i]/ _freqBandHighest[i]);
        }
    }

    private void GetSpectrumDataAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBand()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7) sampleCount += 2;
            
            for (int j = 0; j < sampleCount; j++)
            {
                average += _samples[count] * (count + 1);
                count++;
            }

            average /= count;
            _freqBand[i] = average * 10;
        }
    }

    private void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            } 
            else if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] = 1.2f;
            }
        }
    }

    private void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += _audioBand[i];
            currentAmplitudeBuffer += _audioBandBuffer[i];
        }

        if (currentAmplitude > _amplitudeHighest)
        {
            _amplitudeHighest = currentAmplitude;
        }
    
        _amplitude = currentAmplitude / _amplitudeHighest;
        _amplitudeBuffer = currentAmplitudeBuffer / _amplitudeHighest;

        Debug.Log(_amplitudeBuffer);
    }
}
