/* Copyright (c) 2020 ExT (V.Sigalkin) */

using UnityEngine;


namespace extOSC.Examples
{
    public class tempSend : MonoBehaviour
    {

        public string Address = "/temp";



        public GameObject sliderObject;

        int motorAmount = 6;
        bool[] motorState = new bool[] { false, false, false, false, false, false };



        [Header("OSC Settings")]
        public OSCTransmitter Transmitter;



        protected virtual void Start()
        {

        }

        private void SendTemp(bool isActivated, int index)
        {
            var message = new OSCMessage(Address);
            motorState[index] = isActivated;

            message.AddValue(OSCValue.Float(getTempValue()));

            Debug.Log(getTempValue());

            Transmitter.Send(message);
        }


        // 2022

        public void SendTemp1(bool value) { SendTemp(value, 0); }
        public void SendTemp2(bool value) { SendTemp(value, 1); }
        public void SendTemp3(bool value) { SendTemp(value, 2); }
        public void SendTemp4(bool value) { SendTemp(value, 3); }
        public void SendTemp5(bool value) { SendTemp(value, 4); }
        public void SendTemp6(bool value) { SendTemp(value, 5); }




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