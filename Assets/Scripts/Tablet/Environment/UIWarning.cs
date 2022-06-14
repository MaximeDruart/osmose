using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWarning : MonoBehaviour
{
    // Start is called before the first frame update
    private Material borderMaterial;
    private Material bgMaterial;

    public Color borderWarningColor;
    public Color borderStandardColor;

    public Color TextColor;
    public GameObject background;
    public GameObject TitleBackground;
    private GameObject TitleBackgroundMaterial;
    public TMP_Text TitleText;
    public TMP_Text ToDoText;

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

        t += isWarning ? 0.008f : -0.008f;
        t = Mathf.Clamp01(t);


        Color actualColor = Color.Lerp(borderStandardColor, borderWarningColor, t);
        Color actualBgColor = Color.Lerp(Color.white, Color.red, t);

        Color textColor = Color.Lerp(Color.white, TextColor, t);

        float intensity = extOSC.OSCUtilities.Map(Mathf.Sin(Time.time * 2f), -1, 1, 0.6f, 1) * hdrIntensity;
        float opacity = extOSC.OSCUtilities.Map(Mathf.Sin(Time.time * 2f), -1, 1, 0.5f, 1f);

        if (!isWarning)
        {
            intensity = 1;
            opacity = 1;
        }

        TitleText.color = new Color(textColor.r, textColor.g, textColor.b, opacity);
        ToDoText.color = new Color(textColor.r, textColor.g, textColor.b, opacity);

        borderMaterial.SetColor("_Color", actualColor * intensity);
        bgMaterial.color = actualBgColor * intensity;
    }

    public void EndWarning()
    {
        isWarning = false;
    }
}
