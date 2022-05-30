using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.forward = _camera.transform.position - transform.position; 
    }
}
