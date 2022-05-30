using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOnAmplitude : MonoBehaviour
{
    public float _startPositionX, _startPositionY, _startPositionZ, _maxPositionX, _maxPositionY, _maxPositionZ, _amplitudeLevelTrigger;
    public bool _useBuffer;

    private void Start()
    {
    }

    private void Update()
    {
        if (!_useBuffer)
        {
            transform.localPosition = new Vector3(
                (AudioManager._amplitude * _maxPositionX) + _startPositionX,
                (AudioManager._amplitude * _maxPositionY) + _startPositionY,
                (AudioManager._amplitude * _maxPositionZ) + _startPositionZ
            );
        }
        else if (_useBuffer)
        {
            if (AudioManager._amplitude > _amplitudeLevelTrigger)
            {
                transform.position = new Vector3(
                    (AudioManager._amplitudeBuffer * _maxPositionX) + _startPositionX,
                    (AudioManager._amplitudeBuffer * _maxPositionY) + _startPositionY,
                    (AudioManager._amplitudeBuffer * _maxPositionZ) + _startPositionZ
                );
            }

        }
    }
}
