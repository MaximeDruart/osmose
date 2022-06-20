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
    public Color TitleBackgroundColor;

    public Color TextColor;
    public GameObject background;
    public GameObject TitleBackground;
    private Image TitleBackgroundImage;
    public TMP_Text TitleText;
    public TMP_Text ToDoText;

    [SerializeField]
    private float hdrIntensity;
    private bool isWarning = true;

    private float t = 1f;

    public CompletionState completionState;


    void Start()
    {
        borderMaterial = gameObject.GetComponent<Image>().material;
        bgMaterial = background.GetComponent<Image>().material;
        TitleBackgroundImage = background.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        t += isWarning ? 0.008f : -0.08f;
        t = Mathf.Clamp01(t);


        Color actualColor = Color.Lerp(borderStandardColor, borderWarningColor, t);
        Color actualBgColor = Color.Lerp(Color.white, Color.red, t);

        Color textColor = Color.Lerp(Color.white, TextColor, t);
        Color textBgColor = Color.Lerp(new Color(45, 54, 65), TitleBackgroundColor, t);

        float intensity = extOSC.OSCUtilities.Map(Mathf.Sin(Time.time * 2f), -1, 1, 0.6f, 1) * hdrIntensity;
        float opacity = extOSC.OSCUtilities.Map(Mathf.Sin(Time.time * 2f), -1, 1, 0.5f, 1f);

        if (!isWarning)
        {
            intensity = 1;
            opacity = 1;
        }

        if (t == 0 && completionState.completedModules["Environment"]) return;

        TitleText.color = new Color(textColor.r, textColor.g, textColor.b, opacity);
        TitleBackgroundImage.color = new Color(textBgColor.r, textBgColor.g, textBgColor.b, opacity);
        ToDoText.color = new Color(textColor.r, textColor.g, textColor.b, opacity);

        borderMaterial.SetColor("_Color", actualColor * intensity);
        bgMaterial.color = actualBgColor * intensity;
    }

    public void EndWarning()
    {
        isWarning = false;
    }
}
