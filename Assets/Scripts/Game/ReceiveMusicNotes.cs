using UnityEngine;

namespace extOSC.Examples
{
    public class ReceiveMusicNotes : MonoBehaviour
    {
        // Start is called before the first frame update
        public OSCReceiver Receiver;
        private string Address = "/music";
        private string CompletedAddress = "/music/completed";


        private bool isValidated = false;

        private AudioSource[] audioSources;

        public Animator animator;

        void Start()
        {
            audioSources = GetComponents<AudioSource>();

            Receiver.Bind(Address, ReceiveMusic);
            Receiver.Bind(CompletedAddress, ReceiveCompleted);

        }

        // Update is called once per frame
        void ReceiveMusic(OSCMessage message)
        {
            if (message.ToInt(out var value))
            {
                audioSources[value - 1].Play();
                TriggerDance();
            }
        }
        void ReceiveCompleted(OSCMessage message)
        {
            isValidated = true;
            animator.SetBool("isDance", true);

        }

        private void FixedUpdate()
        {
        }


        void TriggerDance()
        {
            animator.SetTrigger("Dance");
        }
    }

}