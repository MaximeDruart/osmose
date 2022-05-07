using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace extOSC
{
    public class ReceiveDrawing : MonoBehaviour
    {

        [SerializeField]
        OSCReceiver Receiver;

        [SerializeField]
        string Address = "/draw";

        private List<Vector2> points = new List<Vector2>();

        private int activeDrawing = 0;

        // Start is called before the first frame update
        void Start()
        {
            Receiver.Bind(Address, ReceiveMessage);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void onValidDrawing()
        {
            activeDrawing++;
        }


        void ReceiveMessage(OSCMessage message)
        {
            if (message.ToArray(out var arrayValues)) // Get all values from first array in message.
            {
                points.Clear();
                if (arrayValues[0].BoolValue) onValidDrawing();

                // we're sending in a 1D array so we need to split it to get an array of vector2s
                // we're scrolling the array 2 by 2 and creating a vector with the current value and the next
                // 1 4 2 3 2 4
                // -> vec2(1, 4), vec2(2, 3), vec2(2, 4),

                for (int i = 1; i < arrayValues.Count; i += 2)
                {
                    Vector2 newVec = new Vector2(arrayValues[i].FloatValue, arrayValues[i + 1].FloatValue);
                    points.Add(newVec);
                }
            }
        }
    }
}
