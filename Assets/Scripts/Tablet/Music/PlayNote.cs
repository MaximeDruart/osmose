using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayNote : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // Start is called before the first frame update

    private bool isOpened = false;

    private float animationProgressValue = 0f;

    [SerializeField] private float animationSpeed = 0.01f;
    [SerializeField] private GameObject innerCircle;
    [SerializeField] private GameObject points;

    private RectTransform innerCircleRect;
    private RectTransform pointsRect;

    private float innerCircleRectStartSize;
    private float pointsRectStartSize;

    public UnityEvent playFunction;



    void Start()
    {
        innerCircleRect = innerCircle.GetComponent<RectTransform>();
        pointsRect = points.GetComponent<RectTransform>();

        innerCircleRectStartSize = innerCircleRect.rect.width;
        pointsRectStartSize = pointsRect.rect.width;

        Debug.Log("fazfza down");

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
        pointsRect.sizeDelta = Vector2.Lerp(new Vector2(pointsRectStartSize, pointsRectStartSize), new Vector2(52, 52), progress);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isOpened = true;
        playFunction.Invoke();
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
