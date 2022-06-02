using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace extOSC.UI
{
    public class ScaleFrequence : MonoBehaviour
    {
        // Start is called before the first frame update


        [Header("Waves")]
        public GameObject Wave;
        private RectTransform WaveRect;

        public GameObject WaveGlow;
        private RectTransform WaveGlowRect;
        private Material WaveGlowMat;

        public GameObject WaveGlowContainer;


        [Header("Animation")]
        public float waveSize = 15f;
        public float waveSpeed = 1f;

        [Header("Wave Size")]
        public float minWidth;
        public float maxWidth;

        [Space(10)]

        [Range(1f, 10.0f)]
        public float hdrIntensity = 3f;


        private float componentWidth;
        private float framesSinceLastInteraction = 100;
        private float scaleValue;

        private float timer;

        private Color32 white = new Color(255, 255, 255, 255);

        private Color32 transparent = new Color(255, 255, 255, 0);

        private float t = 0;

        void Start()
        {
            WaveRect = Wave.GetComponent<RectTransform>();
            WaveGlowRect = WaveGlow.GetComponent<RectTransform>();
            WaveGlowMat = WaveGlow.GetComponent<Image>().material;
            WaveRect.sizeDelta = new Vector2(maxWidth, WaveRect.rect.height);
            WaveGlowRect.sizeDelta = new Vector2(maxWidth, WaveGlowRect.rect.height);

            componentWidth = WaveGlowContainer.GetComponent<RectTransform>().rect.width;

        }

        // Update is called once per frame
        void Update()
        {


            framesSinceLastInteraction += 1;

            if (framesSinceLastInteraction < 100)
            {
                t += 0.05f;
                timer = Time.time * waveSpeed;
                float width = waveSize * OSCUtilities.Map(scaleValue, 0, 1, 1 + (maxWidth / minWidth), 1);
                AnimateGlowWave(timer, width);

            }
            else
            {
                t -= 0.05f;
            }

            t = Mathf.Clamp01(t);
            WaveGlowMat.SetColor("_Color", Color.Lerp(transparent, white, t) * hdrIntensity);

        }

        void FixedUpdate()
        {

        }

        void AnimateGlowWave(float pos, float width)
        {
            float actualPos = pos % (componentWidth - width);
            Vector4 padding = new Vector4(actualPos, 0, componentWidth - (actualPos + width), 0);
            WaveGlowContainer.GetComponent<RectMask2D>().padding = padding;

        }

        public void Scale(float value)
        {
            scaleValue = value;
            framesSinceLastInteraction = 0;

            WaveRect.sizeDelta = new Vector2(maxWidth - value * (maxWidth - minWidth), WaveRect.rect.height);
            WaveGlowRect.sizeDelta = new Vector2(maxWidth - value * (maxWidth - minWidth), WaveGlowRect.rect.height);
        }

    }
}