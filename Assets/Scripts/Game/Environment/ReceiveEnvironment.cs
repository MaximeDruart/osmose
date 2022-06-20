using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace extOSC.Examples
{
    public class ReceiveEnvironment : MonoBehaviour
    {

        [SerializeField] private OSCReceiver Receiver;
        [SerializeField] private GameObject[] motorObjects;
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


        private void Start()
        {

            skinnedMeshRenderer = EyesObject.GetComponent<SkinnedMeshRenderer>();
            skinnedMesh = EyesObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;

            for (int i = 0; i < motorObjects.Length; i++)
            {
                motorObjectsInitialPosX[i] = motorObjects[i].transform.localPosition.x;

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

        private void onTemperature(OSCMessage message)
        {
            if (message.ToFloat(out float temp))
            {
                // temp value
            }
        }

        private void onPressure(OSCMessage message)
        {
            if (message.ToArray(out var arrayValues))
            {
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

        private void UpdateMotor(int motorIndex, bool isActivated)
        {
            // animate motor object
            if (isActivated)
            {
                motorObjects[motorIndex].transform.DOLocalMoveX(motorObjectsInitialPosX[motorIndex] + motorMovementAmount, 0.6f);
            }
            else
            {
                motorObjects[motorIndex].transform.DOLocalMoveX(motorObjectsInitialPosX[motorIndex], 0.6f);
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
    }
}