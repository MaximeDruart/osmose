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
            if (!string.IsNullOrEmpty(PortController.Instance.remoteIp) && !string.IsNullOrEmpty(PortController.Instance.deviceIp))
            {
                Transmitter.RemoteHost = PortController.Instance.remoteIp;
                Transmitter.LocalHost = PortController.Instance.deviceIp;
                Transmitter.Connect();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}