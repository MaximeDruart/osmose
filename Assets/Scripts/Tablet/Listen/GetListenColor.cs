using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace extOSC.Examples
{
    public class GetListenColor : MonoBehaviour
    {
        // Start is called before the first frame update

        public OSCTransmitter Transmitter;
        public string Address = "/listen/color";

        public Color[] colors;

        public GameObject Handle;
        private Image HandleImage;

        [Header("Validation")]

        public int ValidColorIndex = 4;

        public string AddressCompleted = "/listen/completed";

        public CompletionState completionState;

        public UnityEvent OnCompleted;



        void Start()
        {
            HandleImage = Handle.GetComponent<Image>();
        }

        private void SendValidation()
        {
            var message = new OSCMessage(AddressCompleted);
            message.AddValue(OSCValue.Impulse());

            Transmitter.Send(message);
        }

        private void Validate(int colorIndex)
        {
            if (completionState.completedModules["Listen"]) return;

            if (colorIndex == ValidColorIndex)
            {
                SendValidation();
                OnCompleted.Invoke();
            }
        }

        // Update is called once per frame
        private void SendColor(Color color)
        {
            if (completionState.completedModules["Listen"]) return;

            var message = new OSCMessage(Address);
            message.AddValue(OSCValue.Color(color));

            Transmitter.Send(message);
        }

        public void SendWhite(Vector2 position)
        {
            SendColor(Color.white);
        }

        public void GetColor(Vector2 position)
        {

            float radians = Mathf.Atan2(position.y, position.x);
            float deg = radians * Mathf.Rad2Deg;

            // atan2 is -180deg < atan2 < 180deg but we want between 0 and 360;
            if (deg < 0) deg += 360;
            deg = Mathf.Clamp(deg, 0, 359);


            int colorIndex = GetColorIndexForAngle(deg);
            Color color = colors[colorIndex];

            SetHandleColor(color);
            SendColor(color);

            Validate(colorIndex);
        }


        private int GetColorIndexForAngle(float angle)
        {
            float angleRangeForColor = 360 / colors.Length;
            int colorIndex = Mathf.FloorToInt(angle / angleRangeForColor);

            return colorIndex;

        }

        private void SetHandleColor(Color color)
        {
            HandleImage.material.DOColor(color, 0.3f);
        }
    }

}