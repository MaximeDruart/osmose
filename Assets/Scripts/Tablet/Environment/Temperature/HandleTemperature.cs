using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class HandleTemperature : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public float targetValue = 0.51f;
    public float validationRange = 0.1f;

    public float minPitch = 0.7f;
    public float maxPitch = 1.3f;

    public UnityEvent<bool> validateTemperature;

    private AudioSource audioSource;

    private float maxVolume = 0.7f;

    private IEnumerator coroutine;

    private float currentValue;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetValue(float value)
    {
        currentValue = value;
    }
    public IEnumerator checkForValidation()
    {
        yield return new WaitForSeconds(1f);

        validateTemperature.Invoke(currentValue > targetValue - validationRange && currentValue < targetValue + validationRange);
    }

    public void UpdatePitch(float value)
    {
        audioSource.pitch = extOSC.OSCUtilities.Map(value, 0, 1, minPitch, maxPitch);
        maxVolume = extOSC.OSCUtilities.Map(value, 0, 0.5f, 0.3f, 0.7f);
        audioSource.volume = extOSC.OSCUtilities.Map(value, 0, 0.5f, 0.3f, 1);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!audioSource.isPlaying) audioSource.Play();
        audioSource.DOFade(maxVolume, 0.5f);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        audioSource.DOFade(0, 0.5f);


        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = checkForValidation();
        StartCoroutine(coroutine);


    }
}
