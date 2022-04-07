using UnityEngine;

namespace extOSC.Examples
{
    public class SetPort : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("OSC Settings")]
        public OSCTransmitter Transmitter;


        void Awake()
        {
            if (!string.IsNullOrEmpty(PortController.Instance.desktopIP))
            {
                Transmitter.RemoteHost = PortController.Instance.desktopIP;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}