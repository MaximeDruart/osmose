using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace extOSC.Examples
{
    public class toggleMotor : MonoBehaviour, IPointerClickHandler
    {
        // Start is called before the first frame update

        public bool isOn = true;

        public int index = 0;

        public UnityEvent<bool, int> onToggle;

        [Header("Game Objects")]
        public GameObject MiddleDot;
        public GameObject Border;
        public GameObject CornerSquares;
        public GameObject Background;

        private Image MiddleDotImage;
        private Image BorderImage;
        private Image CornerSquaresImage;
        private Image BackgroundImage;


        void Start()
        {
            MiddleDotImage = MiddleDot.GetComponent<Image>();
            BorderImage = Border.GetComponent<Image>();
            CornerSquaresImage = CornerSquares.GetComponent<Image>();
            BackgroundImage = Background.GetComponent<Image>();

            ToggleAnimation(0);
        }

        public void OnPointerClick(PointerEventData data)
        {
            isOn = !isOn;
            ToggleAnimation();
            onToggle.Invoke(isOn, index);
        }

        void ToggleAnimation(float duration = 0.5f)
        {
            if (isOn)
            {
                BackgroundImage.DOFade(1f, duration);
                MiddleDotImage.DOFade(1f, duration);
                BorderImage.DOFade(1f, duration);
                CornerSquaresImage.DOFade(1f, duration);
            }
            else
            {
                BorderImage.DOFade(0f, duration);
                BackgroundImage.DOFade(0f, duration);
                MiddleDotImage.DOFade(0.6f, duration);
                CornerSquaresImage.DOFade(0.8f, duration);

            }


        }

    }
}