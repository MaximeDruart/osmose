using UnityEngine;
using UnityEngine.Events;

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

        public int CorrectValue = 4;
        public UnityEvent<bool> setValidationStatus;

        public AudioClip audioOn;
        public AudioClip audioOff;

        private AudioSource audioSource;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SendTemp(bool isOn, int index, bool isInitial = false)
        {
            var message = new OSCMessage(Address);
            motorState[index] = isOn;

            setValidationStatus.Invoke((GetNoOfActivatedMotors() == CorrectValue));

            if (!isInitial) TriggerAudio(isOn);

            message.AddValue(OSCValue.Float(GetTempValue()));
            Transmitter.Send(message);
        }


        private void TriggerAudio(bool isOn)
        {
            if (isOn)
            {
                audioSource.PlayOneShot(audioOn, 0.6f);
            }
            else
            {
                audioSource.PlayOneShot(audioOff, 0.6f);

            }
        }



        public void SendArray()
        {
            OSCMessage message = new OSCMessage(Address);
            var array = OSCValue.Array();

            setValidationStatus.Invoke((GetNoOfActivatedMotors() == CorrectValue));

            foreach (bool isActivated in motorState)
            {
                array.AddValue(OSCValue.Bool(isActivated));
            }

            message.AddValue(array);
            Transmitter.Send(message);
        }


        private float GetTempValue()
        {
            float temp = 0f;
            foreach (var motorIsActivated in motorState)
            {
                if (motorIsActivated) temp += (1 / (float)motorAmount);
            }
            return temp;
        }
        private int GetNoOfActivatedMotors()
        {
            int temp = 0;
            foreach (var motorIsActivated in motorState)
            {
                if (motorIsActivated) temp++;
            }
            return temp;
        }
    }
}