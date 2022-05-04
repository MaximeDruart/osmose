using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace extOSC.Examples
{
    public class toggleMotor : MonoBehaviour
    {
        // Start is called before the first frame update
        public string Address = "/temp";

        int motorAmount = 6;
        bool[] motorState = new bool[] { false, false, false, false, false, false };

        public GameObject[] motorsObjects;
        private Material[] motorsMaterials;



        [Header("OSC Settings")]
        public OSCTransmitter Transmitter;


        void Start()
        {

            for (int i = 0; i < motorsObjects.Length; i++)
            {
                Debug.Log(motorsObjects[i].GetComponent<Renderer>().material);
                // motorsMaterials[i] = motorsObjects[i].GetComponent<Renderer>().material;
            }
        }

        public void SendTemp(int index)
        {
            var message = new OSCMessage(Address);
            motorState[index] = !motorState[index];

            updateMotorObject(index, motorState[index]);

            message.AddValue(OSCValue.Float(getTempValue()));
            Transmitter.Send(message);
        }


        private float getTempValue()
        {
            float temp = 0f;
            foreach (var motorIsActivated in motorState)
            {
                if (motorIsActivated) temp += (1 / (float)motorAmount);
            }
            return temp;
        }

        private void updateMotorObject(int index, bool isOn)
        {
            GameObject motor = motorsObjects[index];
            Material mat = motor.GetComponent<Renderer>().material;


            if (isOn)
            {
                mat.DOFloat(1, "_alpha", 1);
                mat.DOColor(Color.black, "_baseColor", 1);
            }
            else
            {
                mat.DOFloat(0.3f, "_alpha", 1);
                mat.DOColor(Color.red, "_baseColor", 1);
            }


        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            RaycastHit hit = Array.Find(hits, (RaycastHit hit) => hit.transform.name.Contains("Cylinder"));

            if (hit.transform)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    char indexStr = hit.transform.name[hit.transform.name.Length - 1];
                    int index = (indexStr - '0') - 1;
                    SendTemp(index);
                }
            }


        }
    }
}