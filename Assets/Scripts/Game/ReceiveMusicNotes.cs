using DG.Tweening;
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

        private string AvaibleFramesAddress = "/music/available";

        private string CancelAvaibleFramesAddress = "/music/available/cancel";


        // private bool isValidated = false;

        private AudioSource[] audioSources;

        public Animator animator;
        public GameObject[] moufles;
        private Material[] mouflesMaterials;
        public UnityEvent OnCompleted;

        void Start()
        {
            audioSources = GetComponents<AudioSource>();

            Receiver.Bind(Address, ReceiveMusic);
            Receiver.Bind(CompletedAddress, ReceiveCompleted);
            Receiver.Bind(CancelAddress, CancelDance);

            Receiver.Bind(AvaibleFramesAddress, SetAvailable);
            Receiver.Bind(CancelAvaibleFramesAddress, CancelAvailable);

            for (int i = 0; i < moufles.Length; i++)
            {
                mouflesMaterials[i] = moufles[i].GetComponent<Renderer>().material;
            }
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

        private void SetAvailable(OSCMessage message)
        {
            if (message.ToInt(out int index))
            {
                mouflesMaterials[index - 1].DOFloat(1, "_isActivated", 0.6f);

                for (int i = 0; i < mouflesMaterials.Length; i++)
                {
                    if (i != index - 1)
                    {
                        mouflesMaterials[i].DOFloat(0, "_isActivated", 0.6f);
                    }
                }

            }
        }
        private void CancelAvailable(OSCMessage message)
        {

            foreach (var moufleMat in mouflesMaterials)
            {
                moufleMat.DOFloat(0, "_isActivated", 0.6f);
            }

        }


        void TriggerDance()
        {
            animator.SetBool("isDance", true);
        }
    }

}