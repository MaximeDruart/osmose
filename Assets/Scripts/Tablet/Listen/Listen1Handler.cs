
using System;
using DG.Tweening;
using extOSC.UI;
using UnityEngine;
using UnityEngine.EventSystems;



namespace extOSC
{

    public class Listen1Handler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [Header("extOSC")]
        public OSCTransmitter Transmitter;
        public string Address = "/listen/frequency/done";


        [Header("Listen Validation")]
        public Vector2 targetValue = new Vector2(0.1f, 0.3f);

        public float validationTreshold = 0.2f;

        [Header("Circle Clamping")]

        public float circleRadius = 1.1f;



        [Header("Objects")]

        [SerializeField]
        private GameObject Pad1Obj;
        private Component Pad1;

        [SerializeField]
        private GameObject ColorPadObj;
        private Component ColorPad;

        [SerializeField]
        private GameObject FreqSliderObj;
        private Component FreqSlider;

        [Header("Audio")]


        [SerializeField] private AudioSource audioSourceNormal;
        [SerializeField] private AudioSource audioSourceDistorted;
        [SerializeField] private float maxDistortionDistance = 3;

        private float volumeMix = 0;


        [Header("Debugging")]

        public bool isValidateEnabled = true;



        void Start()
        {
            Pad1 = Pad1Obj.GetComponent<OSCPad>();
            ColorPad = ColorPadObj.GetComponent<OSCPad>();
            FreqSlider = FreqSliderObj.GetComponent<OSCSlider>();

            ToggleComponentInteract(ColorPad, false);
            ToggleComponentInteract(FreqSlider, false);

        }


        void ToggleComponentInteract(Component component, bool value)
        {
            Type t = component.GetType();
            var p = t.GetProperty("interactable");
            p.SetValue(component, value);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SendConfirm()
        {
            OSCMessage message = new OSCMessage(Address);

            message.AddValue(OSCValue.Impulse());
            Transmitter.Send(message);
        }

        void sendOpacity(float opacity)
        {
            OSCMessage message = new OSCMessage("/listen/opacity");

            message.AddValue(OSCValue.Float(opacity));
            Transmitter.Send(message);
        }

        public void OnPadValueChange(Vector2 inputValue)
        {

            Vector2 value = ConstraintValueToCircle(inputValue);

            float distanceToTarget = Vector2.Distance(targetValue, value);

            HandleAudio(distanceToTarget);


            if (distanceToTarget < validationTreshold && isValidateEnabled)
            {
                ToggleComponentInteract(Pad1, false);
                ToggleComponentInteract(ColorPad, true);
                ToggleComponentInteract(FreqSlider, true);
                sendOpacity(0.5f);
                SendConfirm();
            };
        }



        private Vector2 ConstraintValueToCircle(Vector2 inputValue)
        {

            Vector2 clampedValue = GetPosInCircle(inputValue);

            Type t = Pad1.GetType();
            var p = t.GetProperty("Value");
            p.SetValue(Pad1, clampedValue);

            return clampedValue;

        }


        private Vector2 GetPosInCircle(Vector2 position)
        {
            Vector2 newPos;
            float distanceToCenter = Vector2.Distance(Vector2.zero, position);

            if (distanceToCenter <= circleRadius)
            {
                return position;
            }
            else
            {
                newPos = position;
                double doubleRad = Math.Atan2(position.y, position.x);

                float radians = (float)doubleRad;

                newPos = new Vector2(
                    Mathf.Cos(radians) * circleRadius,
                    Mathf.Sin(radians) * circleRadius
                );

                return newPos;
            }
        }


        private void HandleAudio(float distance)
        {
            volumeMix = OSCUtilities.Map(distance, 0, maxDistortionDistance, 0, 1, true);

            audioSourceNormal.volume = 1 - volumeMix;
            audioSourceDistorted.volume = volumeMix;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!audioSourceNormal.isPlaying)
            {
                audioSourceNormal.Play();
                audioSourceDistorted.Play();
            };
            audioSourceNormal.DOFade(1 - volumeMix, 0.5f);
            audioSourceDistorted.DOFade(volumeMix, 0.5f);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            audioSourceNormal.DOFade(0, 0.5f);
            audioSourceDistorted.DOFade(0, 0.5f);
        }
    }
}

