using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace extOSC.Examples
{
    public class ReceiveEnvironment : MonoBehaviour
    {

        [SerializeField] private OSCReceiver Receiver;
        [SerializeField] private GameObject[] motorObjects;
        private Material[] motorObjectsMats = new Material[6];
        public float motorMovementAmount = 0.2f;

        [SerializeField] private Animator animator;

        [Space(10)]
        [SerializeField] private UnityEvent onComplete;

        [Header("Debugging")]
        [SerializeField] private bool StartDeployed = true;


        private string TemperatureAddress = "/temp";
        private string PressureAddress = "/pressure";

        private string AddressCompleted = "/environment/completed";


        private bool isTempValidated = false;
        private bool isPressureValidated = false;

        private bool[] motorState = new bool[] { false, false, false, false, false, false };

        private float[] motorObjectsInitialPosX = new float[6];

        private AudioSource audioSource;

        public BoolVariable MotionHasPlayed;


        public GameObject EyesObject;

        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Mesh skinnedMesh;


        [Header("Tank UI")]

        public TMP_Text PressureText;
        public TMP_Text TemperatureText;

        private float tempAlpha = 0f;
        private float pressureAlpha = 0f;


        private void Start()
        {

            skinnedMeshRenderer = EyesObject.GetComponent<SkinnedMeshRenderer>();
            skinnedMesh = EyesObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;

            for (int i = 0; i < motorObjects.Length; i++)
            {
                motorObjectsInitialPosX[i] = motorObjects[i].transform.localPosition.x;
                motorObjectsMats[i] = motorObjects[i].GetComponent<Renderer>().material;
            }

            audioSource = GetComponent<AudioSource>();

            AnimateEyes(100, 0);


            if (StartDeployed)
            {
                animator.SetBool("isSemiFetus", true);
                animator.SetBool("isDeployed", true);
                AnimateEyes(0, 0);
            }

            Receiver.Bind(TemperatureAddress, onTemperature);
            Receiver.Bind(PressureAddress, onPressure);

            Receiver.Bind(AddressCompleted, onCompleted);

        }

        private void FixedUpdate()
        {
            tempAlpha -= 0.01f;
            pressureAlpha -= 0.01f;
            tempAlpha = Mathf.Clamp(tempAlpha, 0, 2);
            pressureAlpha = Mathf.Clamp(pressureAlpha, 0, 2);

            PressureText.DOFade(pressureAlpha, 0f);
            TemperatureText.DOFade(tempAlpha, 0f);
        }

        private void onTemperature(OSCMessage message)
        {
            if (message.ToFloat(out float temp))
            {
                SetTemperature(temp);
            }
        }

        private void onPressure(OSCMessage message)
        {
            if (message.ToArray(out var arrayValues))
            {

                SetPressure(GetTempValue());
                if (MotionHasPlayed.Value)
                {
                    PlayAudio();
                }
                for (int i = 0; i < arrayValues.Count; i++)
                {
                    motorState[i] = arrayValues[i].BoolValue;
                    UpdateMotor(i, motorState[i]);
                }

            }
        }

        private float GetTempValue()
        {
            float temp = 0f;
            foreach (var motorIsActivated in motorState)
            {
                if (motorIsActivated) temp += (1 / 6);
            }
            return temp;
        }

        private void UpdateMotor(int motorIndex, bool isActivated)
        {
            // animate motor object
            if (isActivated)
            {
                motorObjects[motorIndex].transform.DOLocalMoveX(motorObjectsInitialPosX[motorIndex] + motorMovementAmount, 0.6f);
                motorObjectsMats[motorIndex].DOFloat(1, "_Progress_Value", 0.6f);
            }
            else
            {
                motorObjects[motorIndex].transform.DOLocalMoveX(motorObjectsInitialPosX[motorIndex], 0.6f);
                motorObjectsMats[motorIndex].DOFloat(0, "_Progress_Value", 0.6f);
            }
        }

        private void onCompleted(OSCMessage message)
        {
            // if creature is deployed dont go back
            if (animator.GetBool("isDeployed")) return;

            isPressureValidated = message.Values[0].BoolValue;
            isTempValidated = message.Values[1].BoolValue;

            // if both are done, it means we're already semi deployed
            if (isTempValidated && isPressureValidated)
            {
                animator.SetBool("isDeployed", true);
                AnimateEyes(0);
                onComplete.Invoke();
            }
            // if only one is done, semi deploy it
            else if (isTempValidated || isPressureValidated)
            {
                animator.SetBool("isSemiFetus", true);
                AnimateEyes(50);

            }
            // if none, go back to fetus
            else
            {
                animator.SetBool("isSemiFetus", false);
                AnimateEyes(100);
            }
        }

        private void PlayAudio()
        {
            audioSource.Play();
        }

        private void AnimateEyes(int endValue, float animationDuration = 3f)
        {
            float currentValue = skinnedMeshRenderer.GetBlendShapeWeight(0);
            int currentValueInt = Mathf.FloorToInt(currentValue);
            DOVirtual
                .Int(currentValueInt, endValue, animationDuration,
                (int i) => skinnedMeshRenderer.SetBlendShapeWeight(0, i)
                )
                .SetDelay(0.5f);
        }

        private void SetTemperature(float temperature)
        {
            tempAlpha += 2;
            float temperatureValue = OSCUtilities.Map(temperature, 0, 1, -20, 12);
            TemperatureText.DOText(temperatureValue.ToString(), 0.5f);
        }
        private void SetPressure(float pressure)
        {
            pressureAlpha += 2;
            float pressureValue = OSCUtilities.Map(pressure, 0, 1, 400, 900);
            PressureText.DOText(pressureValue.ToString(), 0.5f);
        }
    }
}