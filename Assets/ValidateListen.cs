using UnityEngine;
using UnityEngine.Events;



namespace extOSC.Examples
{
    public class ValidateListen : MonoBehaviour
    {
        // Start is called before the first frame update

        public OSCTransmitter Transmitter;
        public string AddressCompleted = "/environment/completed";

        public UnityEvent onValidation;

        private bool isTempValidated = false;
        private bool isPressureValidated = false;

        public CompletionState completionState;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void ValidateTemperature(bool isValidated)
        {
            isTempValidated = isValidated;
            Validate();
        }
        public void ValidatePressure(bool isValidated)
        {
            isPressureValidated = isValidated;
            Validate();
        }

        private void SendValidationState()
        {
            var message = new OSCMessage(AddressCompleted);
            message.AddValue(OSCValue.Bool(isPressureValidated));
            message.AddValue(OSCValue.Bool(isTempValidated));

            Transmitter.Send(message);
        }



        private void Validate()
        {
            if (completionState.completedModules["Environment"]) return;

            SendValidationState();

            if (isTempValidated && isPressureValidated)
            {
                onValidation.Invoke();
            }
        }
    }

}