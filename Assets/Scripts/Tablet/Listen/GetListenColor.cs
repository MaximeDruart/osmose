using DG.Tweening;
using UnityEngine;
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

        void Start()
        {
            HandleImage = Handle.GetComponent<Image>();
        }

        // Update is called once per frame

        public void GetColor(Vector2 position)
        {

            float radians = Mathf.Atan2(position.y, position.x);
            float deg = radians * Mathf.Rad2Deg;

            // atan2 is -180deg < atan2 < 180deg but we want between 0 and 360;
            if (deg < 0) deg += 360;
            deg = Mathf.Clamp(deg, 0, 359);


            Color color = GetColorForAngle(deg);
            SetHandleColor(color);
            SendColor(color);

        }

        private void SendColor(Color color)
        {
            var message = new OSCMessage(Address);
            message.AddValue(OSCValue.Color(color));

            Transmitter.Send(message);
        }

        private Color GetColorForAngle(float angle)
        {
            float angleRangeForColor = 360 / colors.Length;
            int colorIndex = Mathf.FloorToInt(angle / angleRangeForColor);

            return colors[colorIndex];

        }

        private void SetHandleColor(Color color)
        {
            HandleImage.material.DOColor(color, 0.3f);
        }
    }

}