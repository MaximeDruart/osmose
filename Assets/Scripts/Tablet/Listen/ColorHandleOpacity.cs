using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ColorHandleOpacity : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject HandleWhite;
    private Image HandleWhiteImage;
    public GameObject HandleColor;
    private Image HandleColorImage;
    public GameObject HandleColorInner;
    private Image HandleColorInnerImage;

    bool colorIsActivated = true;

    void Start()
    {
        HandleWhiteImage = HandleWhite.GetComponent<Image>();
        HandleColorImage = HandleColor.GetComponent<Image>();
        HandleColorInnerImage = HandleColorInner.GetComponent<Image>();

        SetColorMode();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetColorMode()
    {
        if (!colorIsActivated)
        {
            colorIsActivated = true;
            HandleWhiteImage.material.DOFade(0, 0.15f);

            HandleColorImage.material.DOFade(1, 0.15f);
            HandleColorInnerImage.material.DOFade(1, 0.15f);
        }
    }
    public void SetWhiteMode()
    {
        if (colorIsActivated)
        {
            colorIsActivated = false;
            HandleWhiteImage.material.DOFade(1, 0.15f);

            HandleColorImage.material.DOFade(0, 0.15f);
            HandleColorInnerImage.material.DOFade(0, 0.15f);

        }

    }
}
