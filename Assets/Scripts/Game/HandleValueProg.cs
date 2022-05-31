using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HandleValueProg : MonoBehaviour
{
    // Start is called before the first frame update
    private float endValue = 4.8f;

    public UnityEvent onValidate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleValue(float y) {
        Vector3 newVec = gameObject.transform.position;
        newVec.y = y * endValue;
        gameObject.transform.position = newVec;

        if (y > 0.5 && y < 0.7) onValidate.Invoke();
    }
}
