using UnityEngine;
using UnityEngine.Events;

namespace extOSC.Examples
{
    public class ReceiveMusicNotes : MonoBehaviour
    {
        // Start is called before the first frame update
        public OSCReceiver Receiver;
        private string Address = "/music";
        private string CompletedAddress = "/music/completed";
        private string CancelAddress = "/music/cancel";


        // private bool isValidated = false;

        private AudioSource[] audioSources;

        public Animator animator;
        public UnityEvent OnCompleted;

        void Start()
        {
            audioSources = GetComponents<AudioSource>();

            Receiver.Bind(Address, ReceiveMusic);
            Receiver.Bind(CompletedAddress, ReceiveCompleted);
            Receiver.Bind(CancelAddress, CancelDance);

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

            OnCompleted.Invoke();
            // isValidated = true;
            // animator.SetBool("isDance", true);
        }


        private void CancelDance(OSCMessage message)
        {
            animator.SetBool("isDance", false);
        }


        void TriggerDance()
        {
            animator.SetBool("isDance", true);
        }
    }

}