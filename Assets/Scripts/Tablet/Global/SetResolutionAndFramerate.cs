using UnityEngine;

public class SetResolutionAndFramerate : MonoBehaviour
{
    // Start is called before the first frame update
    private int nativeWidth = 2736;
    private int nativeHeight = 1824;

    [Header("Resolution")]

    public bool isActivated = false;

    [Range(0.1f, 1f)]
    public float resolutionPct = 0.6f;

    [Space(10)]

    [Range(0, 165)]
    public int targetFrameRate = 60;

    void Start()
    {
        if (targetFrameRate != 0)
        {
            Debug.Log("setting target framerta");
            Application.targetFrameRate = targetFrameRate;
        }

        if (!isActivated) return;

        // matching the surface format which is 3/2;
        // native resolution is 2736 * 1824 which leads to low fps so we're aiming for less

        Debug.Log(Mathf.CeilToInt(nativeWidth * resolutionPct));

        Screen.SetResolution(Mathf.CeilToInt(nativeWidth * resolutionPct), Mathf.CeilToInt(nativeHeight * resolutionPct), FullScreenMode.ExclusiveFullScreen, targetFrameRate);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
