/* Copyright (c) 2020 ExT (V.Sigalkin) */

using UnityEngine;


namespace extOSC.Examples
{
    public class tempController : MonoBehaviour
    {
        #region Public Vars

        public string RootAddress = "/temp";



        public GameObject sliderObject;

        private float tempValue = 1;
        private float lerpValue;



        [Header("OSC Settings")]
        public OSCReceiver Receiver;

        #endregion

        #region Unity Methods

        protected virtual void Start()
        {
            lerpValue = tempValue;

            Receiver.Bind(RootAddress, ReceivedMessage);

        }

        #endregion

        #region Private Methods

        private void ReceivedMessage(OSCMessage message)
        {
            if (message.ToFloat(out var value))
            {
                tempValue = value;
            }
        }


        private void FixedUpdate()
        {

            lerpValue = Mathf.Lerp(lerpValue, tempValue, .1f);
        }

        #endregion
    }
}