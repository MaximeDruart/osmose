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
            Debug.Log(PortController.Instance.desktopIP);
            if (!string.IsNullOrEmpty(PortController.Instance.desktopIP) && !string.IsNullOrEmpty(PortController.Instance.deviceIp))
            {
                Transmitter.RemoteHost = PortController.Instance.desktopIP;
                Transmitter.LocalHost = PortController.Instance.deviceIp;
                Transmitter.Connect();
                Debug.Log(Transmitter.IsStarted);

            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}