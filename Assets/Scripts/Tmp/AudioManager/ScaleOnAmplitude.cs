using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public float _startScale, _maxScale;
    public bool _useBuffer;

    private void Start()
    {
    }

    private void Update()
    {
        if (!_useBuffer)
        {
            transform.localScale = new Vector3 (
                (AudioManager._amplitude * _maxScale) + _startScale,
                (AudioManager._amplitude * _maxScale) + _startScale,
                (AudioManager._amplitude * _maxScale) + _startScale
            );
        }
        else if (_useBuffer)
        {
            transform.localScale = new Vector3 (
                (AudioManager._amplitudeBuffer * _maxScale) + _startScale,
                (AudioManager._amplitudeBuffer * _maxScale) + _startScale,
                (AudioManager._amplitudeBuffer * _maxScale) + _startScale
            );
        } 
    }
}
