using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



namespace extOSC.Examples
{
    public class HandleLoader : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {


        [Header("extOSC")]

        public OSCTransmitter Transmitter;

        [Header("Game Objects")]

        public GameObject Canvas;
        public GameObject Thumb;
        public GameObject LineProgression;
        public GameObject Trail;
        public GameObject Background;
        private Image LineProgressionImage;

        private float progressionValue = 0f;
        private float increment = 0.01f;

        private bool isHolding = false;

        private bool isCompleted = false;

        [Space(10)]
        public BoolVariable HelpIsOpened;




        void Start()
        {
            LineProgressionImage = LineProgression.GetComponent<Image>();
            HelpIsOpened.Value = true;
        }

        private void FixedUpdate()
        {
            if (isCompleted) return;

            if (isHolding)
            {
                progressionValue += increment;
            }
            else
            {
                progressionValue -= increment;
            }

            progressionValue = Mathf.Clamp01(progressionValue);
            if (progressionValue == 1) HideLoader();

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            LineProgressionImage.fillAmount = progressionValue;
        }
        private void HideLoader()
        {
            isCompleted = true;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(Trail.GetComponent<Image>().DOFade(0f, 0.6f));
            mySequence.Join(LineProgression.GetComponent<Image>().DOFade(0f, 0.6f));
            mySequence.Join(Thumb.GetComponent<Image>().DOFade(0f, 0.6f));

            mySequence.Append(Background.GetComponent<Image>().DOFade(0f, 0.6f));

            mySequence.OnComplete(() =>
            {
                Canvas.SetActive(false);
                HelpIsOpened.Value = false;
            });
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isHolding = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isHolding = false;
        }

    }

}