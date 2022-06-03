using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace extOSC.Examples
{
    public class HandleMotors : MonoBehaviour
    {
        // Start is called before the first frame update

        [Header("OSC Settings")]
        public OSCTransmitter Transmitter;
        public string Address = "/pressure";

        int motorAmount = 6;
        bool[] motorState = new bool[] { false, false, false, false, false, false };

        void Start()
        {

        }

        public void SendTemp(bool isOn, int index)
        {
            var message = new OSCMessage(Address);
            motorState[index] = isOn;

            message.AddValue(OSCValue.Float(getTempValue()));
            Transmitter.Send(message);
        }

        public void SendArray()
        {
            OSCMessage message = new OSCMessage(Address);
            var array = OSCValue.Array();

            foreach (bool isActivated in motorState)
            {
                array.AddValue(OSCValue.Bool(isActivated));
            }

            message.AddValue(array);
            Transmitter.Send(message);
        }


        private float getTempValue()
        {
            float temp = 0f;
            foreach (var motorIsActivated in motorState)
            {
                if (motorIsActivated) temp += (1 / (float)motorAmount);
            }
            return temp;
        }
    }
}