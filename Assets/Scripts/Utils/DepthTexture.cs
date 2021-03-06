using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthTexture : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _camera.depthTextureMode = DepthTextureMode.Depth;
    }
}
