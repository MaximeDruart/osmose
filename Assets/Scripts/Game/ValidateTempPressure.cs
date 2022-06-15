using DG.Tweening;
using UnityEngine;

namespace extOSC.Examples
{
    public class ValidateTempPressure : MonoBehaviour
    {

        public OSCReceiver Receiver;
        public string TemperatureAddress = "/temp";
        public string TemperatureAddressCompleted = "/temp/completed";
        public string PressureAddress = "/pressure";
        public string PressureAddressCompleted = "/pressure/completed";

        private bool[] motorState = new bool[] { false, false, false, false, false, false };

        public GameObject[] motorObjects;
        public float[] motorObjectsInitialPosX;
        public float motorMovementAmount = 0.2f;

        [SerializeField] private Animator animator;


        private void Start()
        {
            Receiver.Bind(TemperatureAddress, onTemperature);
            Receiver.Bind(TemperatureAddressCompleted, onTemperatureCompleted);

            Receiver.Bind(PressureAddress, onPressure);
            Receiver.Bind(PressureAddressCompleted, onPressureCompleted);


            for (int i = 0; i < motorObjects.Length; i++)
            {
                motorObjectsInitialPosX[i] = motorObjects[i].transform.localPosition.x;

            }

        }

        private void onTotalCompletion()
        {
            animator.SetBool("isDeployed", true);
        }

        private void onTemperature(OSCMessage message)
        {
            if (message.ToFloat(out float temp))
            {
                // temp value
            }
        }
        private void onTemperatureCompleted(OSCMessage message)
        {
            //
        }
        private void onPressure(OSCMessage message)
        {
            if (message.ToArray(out var arrayValues))
            {
                for (int i = 0; i < arrayValues.Count; i++)
                {
                    motorState[i] = arrayValues[i].BoolValue;
                    UpdateMotor(i, motorState[i]);
                }

            }
        }

        private void UpdateMotor(int motorIndex, bool isActivated)
        {
            // animate motor object
            if (isActivated)
            {
                motorObjects[motorIndex].transform.DOLocalMoveX(motorObjectsInitialPosX[motorIndex] - motorMovementAmount, 0.6f);
            }
            else
            {
                motorObjects[motorIndex].transform.DOLocalMoveX(motorObjectsInitialPosX[motorIndex], 0.6f);
            }
        }

        private void onPressureCompleted(OSCMessage message)
        {

        }
    }
}