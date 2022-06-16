using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    // Start is called before the first frame update

    private RectTransform rect;

    public float loopSize = 270;

    private float currentScrollValue = 0f;

    [Range(0, 7)]
    public float speed = 1f;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        currentScrollValue += speed;
        if (currentScrollValue > loopSize) currentScrollValue = 0;

        rect.anchoredPosition = Vector3.left * currentScrollValue;
    }
}
