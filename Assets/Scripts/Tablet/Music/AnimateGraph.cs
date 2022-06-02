using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class AnimateGraph : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject WaveGlow;
    public GameObject WaveGlowContainer;
    private RectTransform WaveGlowRect;

    public float duration;

    private float componentWidth;

    private float width = 10f;

    void Start()
    {
        WaveGlowRect = WaveGlow.GetComponent<RectTransform>();
        componentWidth = WaveGlowContainer.GetComponent<RectTransform>().rect.width;
        WaveGlowContainer.GetComponent<RectMask2D>().padding = new Vector4(0, 0, componentWidth, 0);

    }


    void AnimateGlowWave(float pos)
    {

        Vector4 padding = new Vector4(pos, 0, Mathf.Max(componentWidth - (pos + width), 0), 0);
        WaveGlowContainer.GetComponent<RectMask2D>().padding = padding;

    }

    public void StartAnimation()
    {
        DOVirtual.Float(0, componentWidth, duration, AnimateGlowWave).SetEase(Ease.Linear);
    }
}
