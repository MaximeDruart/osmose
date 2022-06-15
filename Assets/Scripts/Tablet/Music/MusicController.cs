using UnityEngine;
using UnityEngine.Events;

namespace extOSC.Examples
{
    public class MusicController : MonoBehaviour
    {
        // Start is called before the first frame update
        public OSCTransmitter Transmitter;
        private string Address = "/music";

        private string CompletedAddress = "/music/completed";
        private string CancelAddress = "/music/cancel";
        private string AvaibleFramesAddress = "/music/available";
        private string CancelAvaibleFramesAddress = "/music/available/cancel";



        public bool playOnTablet = false;
        private AudioSource[] audioSources;

        // private float delaySinceLastNote = 0f;
        private int consecutiveNotes = 0;
        private float startTime = 0f;
        private float delaySinceLastNote = 0f;

        public Vector2 minMaxNoteDelay;

        private bool isComponentValidated = false;


        public UnityEvent onValidation;

        void Start()
        {
            audioSources = GetComponents<AudioSource>();
        }
        void Update()
        {
            delaySinceLastNote = Time.time - startTime;

            // if delay is in the correct frames, light up corresponding moufle
            if (delaySinceLastNote > minMaxNoteDelay.x && delaySinceLastNote < minMaxNoteDelay.y)
            {
                SendIsAvailable();
            }

            // if max delay is exceeded, turn off moufle
            if (delaySinceLastNote > minMaxNoteDelay.y)
            {
                SendCancelIsAvailable();
            }
        }



        public void playSound1(bool isTrue)
        {
            if (isTrue)
            {
                if (playOnTablet)
                {
                    audioSources[0].Play();
                }
                SendNote(1);

            }
        }
        public void playSound2(bool isTrue)
        {
            if (isTrue)
            {
                if (playOnTablet)
                {
                    audioSources[1].Play();
                }
                SendNote(2);

            }
        }
        public void playSound3(bool isTrue)
        {
            if (isTrue)
            {
                if (playOnTablet)
                {
                    audioSources[2].Play();
                }
                SendNote(3);

            }
        }

        private void SendNote(int i)
        {
            // SEND MUSIC NOTE
            var message = new OSCMessage(Address);
            message.AddValue(OSCValue.Int(i));

            Transmitter.Send(message);


            // VALIDATE

            if (isComponentValidated) return;

            bool isValidated = Validate();

            if (isValidated)
            {
                var validationMessage = new OSCMessage(CompletedAddress);
                validationMessage.AddValue(OSCValue.Impulse());
                Transmitter.Send(validationMessage);

                isComponentValidated = true;

                onValidation.Invoke();
            }
        }
        private void SendCancel()
        {
            // SEND MUSIC NOTE
            var message = new OSCMessage(CancelAddress);
            message.AddValue(OSCValue.Impulse());

            Transmitter.Send(message);
        }
        private void SendIsAvailable()
        {
            // SEND MUSIC NOTE
            var message = new OSCMessage(AvaibleFramesAddress);
            message.AddValue(OSCValue.Int(consecutiveNotes));

            Transmitter.Send(message);
        }
        private void SendCancelIsAvailable()
        {
            // SEND MUSIC NOTE
            var message = new OSCMessage(CancelAvaibleFramesAddress);
            message.AddValue(OSCValue.Impulse());

            Transmitter.Send(message);
        }

        private bool Validate()
        {
            bool isNoteValidated = false;

            if (consecutiveNotes == 0)
            {
                startTime = Time.time;
            }

            delaySinceLastNote = Time.time - startTime;


            if ((delaySinceLastNote > minMaxNoteDelay.x && delaySinceLastNote < minMaxNoteDelay.y) || delaySinceLastNote == 0)
            {
                isNoteValidated = true;
                consecutiveNotes++;
                startTime = Time.time;
            }

            if (!isNoteValidated)
            {
                consecutiveNotes = 0;
                SendCancel();
            }

            return consecutiveNotes >= 4;
        }

    }
}