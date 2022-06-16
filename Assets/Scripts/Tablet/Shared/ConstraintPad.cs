using System;
using DG.Tweening;
using extOSC.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace extOSC.Examples
{
    public class ConstraintPad : MonoBehaviour
    {
        // Start is called before the first frame update

        private Component Pad;

        public float circleRadius = 1f;
        public float innerCircleRadius = 0.6f;


        void Start()
        {
            Pad = GetComponent<OSCPad>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPadValueChange(Vector2 inputValue)
        {

            Vector2 value = ConstraintValueToCircle(inputValue);

        }


        private Vector2 ConstraintValueToCircle(Vector2 inputValue)
        {

            Vector2 clampedValue = GetPosInCircle(inputValue);

            Type t = Pad.GetType();
            var p = t.GetProperty("Value");
            p.SetValue(Pad, clampedValue);

            return clampedValue;

        }


        private Vector2 GetPosInCircle(Vector2 position)
        {
            Vector2 newPos;
            float distanceToCenter = Vector2.Distance(Vector2.zero, position);

            if (distanceToCenter <= circleRadius && distanceToCenter >= innerCircleRadius)
            {
                return position;
            }
            else
            {
                newPos = position;
                if (distanceToCenter >= circleRadius)
                {
                    double doubleRad = Math.Atan2(position.y, position.x);

                    float radians = (float)doubleRad;

                    newPos = new Vector2(
                        Mathf.Cos(radians) * circleRadius,
                        Mathf.Sin(radians) * circleRadius
                    );

                }

                if (distanceToCenter <= innerCircleRadius)
                {
                    double doubleRad = Math.Atan2(position.y, position.x);

                    float radians = (float)doubleRad;

                    newPos = new Vector2(
                        Mathf.Cos(radians) * innerCircleRadius,
                        Mathf.Sin(radians) * innerCircleRadius
                    );
                }

                return newPos;


            }
        }
    }

}