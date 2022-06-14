using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace extOSC.Examples
{
    public class ValidateTempPressure : MonoBehaviour
    {

        public OSCReceiver Receiver;
        public string TemperatureAddress = "/temp";
        public string TemperatureAddressCompleted = "/temp/completed";
        public string PressureAddress = "/pressure";
        public string PressureAddressCompleted = "/pressure/completed";

        private bool[] motorState = new bool[] { false, false, false, false, false, false };

        public GameObject[] motorObjects;

        [SerializeField] private Animator animator;


        private void Start()
        {
            Receiver.Bind(TemperatureAddress, onTemperature);
            Receiver.Bind(TemperatureAddressCompleted, onTemperatureCompleted);

            Receiver.Bind(PressureAddress, onPressure);
            Receiver.Bind(PressureAddressCompleted, onPressureCompleted);

        }

        private void onValidTempPressure()
        {

        }
        private void onTemperature(OSCMessage message)
        {
            if (message.ToFloat(out float temp))
            {
                // temp value
            }
        }
        private void onTemperatureCompleted(OSCMessage message)
        {
            //
        }
        private void onPressure(OSCMessage message)
        {
            if (message.ToArray(out var arrayValues))
            {
                for (int i = 0; i < arrayValues.Count; i++)
                {
                    motorState[i] = arrayValues[i].BoolValue;
                    UpdateMotor(i);
                }

            }
        }

        private void UpdateMotor(int motorIndex)
        {
            // animate motor object
            // motorObjects[i]
        }

        private void onPressureCompleted(OSCMessage message)
        {

        }
    }
}