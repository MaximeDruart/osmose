using UnityEngine;
using System.Collections;

public class MotorMovement : MonoBehaviour
{

    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private Vector3 _position;
    private bool _isRotating;

    public float minScaleDelta = 0f;
    public float maxScaleDelta = 3f;

    public Vector2 rotationXLimits = new Vector2(-80, 0);
    public Vector2 rotationYLimits = new Vector2(-80, 80);


    void Start()
    {
        _sensitivity = 0.4f;
        _rotation = transform.rotation.eulerAngles;
        _position = transform.position;

        minScaleDelta = transform.localScale.x - minScaleDelta;
        maxScaleDelta = transform.localScale.x + maxScaleDelta;
    }

    void Update()
    {

        // if (Input.GetMouseButtonDown(0))
        // {
        //     zoom(0.1f);
        // }
        // if (Input.GetMouseButtonDown(1))
        // {
        //     zoom(-0.1f);
        // }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }

        if (_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);

            // apply rotation
            _rotation.y = _rotation.y - (_mouseOffset.x) * _sensitivity;
            _rotation.x = _rotation.x - (_mouseOffset.y) * _sensitivity;

            _rotation.x = Mathf.Clamp(_rotation.x, rotationXLimits.x, rotationXLimits.y);
            // _rotation.y = Mathf.Clamp(_rotation.y, rotationYLimits.x, rotationYLimits.y);

            transform.eulerAngles = _rotation;

            // store mouse
            _mouseReference = Input.mousePosition;
        }
    }

    void zoom(float increment)
    {
        Vector3 scale = transform.localScale;
        float newScale = scale.x + increment;
        newScale = Mathf.Clamp(newScale, minScaleDelta, maxScaleDelta);
        scale.x = newScale;
        scale.y = newScale;
        scale.z = newScale;

        transform.localScale = scale;
    }

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;

        // store mouse
        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }

}