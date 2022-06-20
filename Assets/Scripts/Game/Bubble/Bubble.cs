using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float destroyLimit = 10f;
    [SerializeField] private float bubbleFlySpeed = 0.1f;
    [SerializeField] private Vector3 bubbleEndScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 bubbleScaleGrowing = new Vector3(0.00001f, 0.00001f, 0.00001f);
    
    void Start()
    {
    }

    void Update()
    {
        transform.Translate(new Vector3(0, bubbleFlySpeed, 0));

        int randomInt = Random.Range(1, 10);
        if(randomInt % 2 == 0)
        {
            transform.localScale += bubbleScaleGrowing;
        } 
        else 
        { 
            transform.localScale -= bubbleScaleGrowing; 
        }

        if (transform.position.y > destroyLimit || transform.localScale == bubbleEndScale)
        {
            Destroy(gameObject);
        }
    }
}
