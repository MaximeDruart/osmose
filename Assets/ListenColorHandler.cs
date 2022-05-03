
using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using extOSC.Core.Reflection;
using extOSC.UI;


namespace extOSC
{

    public class ListenColorHandler : MonoBehaviour
    {

        public OSCTransmitter Transmitter;
        public string OpacityAddress = "/listen/opacity";
        public string ConfirmAddress = "/listen/opacity/done";
        public Vector2 targetValue = new Vector2(0.1f, 0.3f);

        public float validationTreshold = 0.03f;
        public float opacityRadius = 0.45f;




        [SerializeField]
        private GameObject ColorPadObj;
        private Component ColorPad;

        // Start is called before the first frame update


        void Start()
        {
            ColorPad = ColorPadObj.GetComponent<OSCPad>();
        }

        void ToggleComponentInteract(Component component, bool value)
        {
            Type t = component.GetType();
            var p = t.GetProperty("interactable");
            p.SetValue(component, value);

        }



        void sendOpacity(float opacity)
        {
            OSCMessage message = new OSCMessage(OpacityAddress);

            message.AddValue(OSCValue.Float(opacity));
            Transmitter.Send(message);
        }
        void SendConfirm()
        {
            OSCMessage message = new OSCMessage(ConfirmAddress);

            message.AddValue(OSCValue.Impulse());
            Transmitter.Send(message);


        }

        public float MapRange(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public void OnPadValueChange(Vector2 value)
        {
            float distanceToTarget = Vector2.Distance(targetValue, value);

            Debug.Log(distanceToTarget);


            float opacity = opacityRadius - distanceToTarget;

            opacity = Mathf.Clamp(opacity, 0, 1);

            opacity = MapRange(opacity, 0, opacityRadius, 0.5f, 1);

            sendOpacity(opacity);


            if (distanceToTarget < validationTreshold)
            {
                ToggleComponentInteract(ColorPad, false);
                SendConfirm();
                sendOpacity(1f);
            };
        }
    }
}

