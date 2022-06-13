using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    // Start is called before the first frame update
    private int nativeWidth = 2736;
    private int nativeHeight = 1824;

    [Range(0.1f, 1f)]
    public float resolutionPct = 0.6f;

    public bool isActivated = false;

    void Start()
    {
        if (!isActivated) return;
        // Application.TargetFramerate = 60;
        // matching the surface format which is 3/2;
        // native resolution is 2736 * 1824 which leads to low fps so we're aiming for less
        Screen.SetResolution(Mathf.CeilToInt(nativeWidth * resolutionPct), Mathf.CeilToInt(nativeHeight * resolutionPct), FullScreenMode.ExclusiveFullScreen, 60);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
