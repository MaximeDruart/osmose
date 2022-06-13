using UnityEngine;

namespace extOSC.Examples
{
    public class ReceiveMusicNotes : MonoBehaviour
    {
        // Start is called before the first frame update
        public OSCReceiver Receiver;
        public string Address = "/music/";

        private AudioSource[] audioSources;

        void Start()
        {
            audioSources = GetComponents<AudioSource>();

            Receiver.Bind(Address + "1", ReceiveMusic1);
            Receiver.Bind(Address + "2", ReceiveMusic2);
            Receiver.Bind(Address + "3", ReceiveMusic3);

        }

        // Update is called once per frame
        void ReceiveMusic1(OSCMessage message)
        {
            audioSources[0].Play();
        }
        void ReceiveMusic2(OSCMessage message)
        {
            audioSources[1].Play();
        }
        void ReceiveMusic3(OSCMessage message)
        {
            audioSources[2].Play();
        }
    }

}