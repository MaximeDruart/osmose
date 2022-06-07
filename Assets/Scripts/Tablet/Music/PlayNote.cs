using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayNote : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update

    private bool isOpened = false;

    private float animationProgressValue = 0f;

    [SerializeField] private float animationSpeed = 0.01f;
    [SerializeField] private GameObject innerCircle;
    [SerializeField] private GameObject innerCircle2;

    private RectTransform innerCircleRect;
    private RectTransform innerCircle2Rect;

    private float innerCircleRectStartSize;
    private float innerCircle2RectStartSize;

    public UnityEvent<bool> playFunction;



    void Start()
    {
        innerCircleRect = innerCircle.GetComponent<RectTransform>();
        innerCircle2Rect = innerCircle2.GetComponent<RectTransform>();

        innerCircleRectStartSize = innerCircleRect.rect.width;
        innerCircle2RectStartSize = innerCircle2Rect.rect.width;

    }

    // Update is called once per frame
    void Update()
    {



        animationProgressValue = isOpened ? animationProgressValue + animationSpeed : animationProgressValue - animationSpeed;
        animationProgressValue = Mathf.Clamp(animationProgressValue, 0f, 1f);

        UpdateAnim(easeOutExpo(animationProgressValue));
    }

    void UpdateAnim(float progress)
    {
        innerCircleRect.sizeDelta = Vector2.Lerp(new Vector2(innerCircleRectStartSize, innerCircleRectStartSize), new Vector2(65, 65), progress);
        innerCircle2Rect.sizeDelta = Vector2.Lerp(new Vector2(innerCircle2RectStartSize, innerCircle2RectStartSize), new Vector2(52, 52), progress);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isOpened = true;
        playFunction.Invoke(true);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isOpened = false;
    }

    private float easeOutExpo(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
}
