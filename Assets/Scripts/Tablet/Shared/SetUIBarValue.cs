using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SetUIBarValue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Range(0, 100)]
    private int value = 95;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private GameObject ProgressionBar;

    [SerializeField]
    private GameObject ProgressionContainer;

    private RectTransform ProgressionContainerRect;
    private RectTransform ProgressionBarRect;



    void Start()
    {
        ProgressionContainerRect = ProgressionContainer.GetComponent<RectTransform>();
        ProgressionBarRect = ProgressionBar.GetComponent<RectTransform>();

        setText(value);
        SetWidth(value);
    }


    void SetWidth(float width)
    {
        ProgressionBarRect.sizeDelta = new Vector2(width * 0.01f * ProgressionContainerRect.rect.width, ProgressionBarRect.rect.height);
    }

    public void setText(float value)
    {
        text.text = value.ToString();
    }
}
