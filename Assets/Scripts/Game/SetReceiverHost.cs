using UnityEngine;

namespace extOSC.Examples
{
    public class SetReceiverHost : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("OSC Settings")]
        public OSCReceiver Receiver;


        void Awake()
        {
            if (!string.IsNullOrEmpty(PortController.Instance.desktopIP))
            {
                Receiver.LocalHost = PortController.Instance.desktopIP;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}