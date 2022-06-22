using UnityEngine;
using UnityEngine.SceneManagement;

namespace extOSC.Examples
{
    public class ResetApp : MonoBehaviour
    {

        [Header("extOSC")]
        public OSCTransmitter Transmitter;
        public string Address = "/reset";

        public void Reset(bool _)
        {
            SendReset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }

        private void SendReset()
        {
            var message = new OSCMessage(Address);
            message.AddValue(OSCValue.Impulse());

            Transmitter.Send(message);
        }

        public void ResetTablet()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }
    }

}