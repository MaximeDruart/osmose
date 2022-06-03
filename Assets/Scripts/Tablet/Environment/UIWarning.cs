using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWarning : MonoBehaviour
{
    // Start is called before the first frame update
    private Material borderMaterial;
    private Material bgMaterial;

    public Color borderWarningColor;
    public Color borderStandardColor;
    public GameObject background;

    [SerializeField]
    private float hdrIntensity;
    private bool isWarning = true;

    float t = 1f;


    void Start()
    {
        borderMaterial = gameObject.GetComponent<Image>().material;
        bgMaterial = background.GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {

        t += isWarning ? 0.05f : -0.05f;

        Color actualColor = Color.Lerp(borderStandardColor, borderWarningColor, t);

        float intensity = extOSC.OSCUtilities.Map(Mathf.Sin(Time.time * 2f), -1, 1, 0.4f, 1) * hdrIntensity;

        if (!isWarning) intensity = 1;

        borderMaterial.SetColor("_Color", actualColor * intensity);
        // bgMaterial.color = Color.Lerp(Color.white, Color.red * intensity, t);
    }

    public void EndWarning()
    {
        isWarning = false;
    }
}
