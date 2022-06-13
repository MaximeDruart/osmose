using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ToggleHelp : MonoBehaviour
{

    private bool isOpened = false;

    public GameObject Container;
    private RectTransform ContainerRect;

    public GameObject Sidebar;
    private RectTransform SidebarRect;

    public GameObject Content;
    private RectTransform ContentRect;

    public GameObject PlaneFilter;
    private Material PlaneFilterMat;


    [Header("Button")]
    public GameObject ButtonOpen;
    public GameObject ButtonClosed;

    public TMP_Text text;

    [Space(10)]

    public BoolVariable HelpIsOpened;


    private Sequence mySequence;
    // Start is called before the first frame update
    void Start()
    {
        SidebarRect = Sidebar.GetComponent<RectTransform>();
        ContentRect = Content.GetComponent<RectTransform>();
        ContainerRect = Container.GetComponent<RectTransform>();
        PlaneFilterMat = PlaneFilter.GetComponent<Renderer>().material;
        PlaneFilterMat.DOFade(0f, 0f);
        // mySequence = DOTween.Sequence();
        // mySequence.Append(SidebarRect.DOLocalMoveX(120, 0.3f));
        // mySequence.Pause();
        Content.GetComponent<Image>().DOFade(0f, 0f);
        Content.SetActive(false);

        ToggleImage(0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle(bool _)
    {
        isOpened = !isOpened;
        HelpIsOpened.Value = isOpened;
        ToggleImage();
        if (isOpened)
        {

            Sequence mySeq = DOTween.Sequence();
            mySeq.Append(ContainerRect.DOLocalMoveX(-428, 0.3f));
            mySeq.Join(PlaneFilterMat.DOFade(0.8f, 0.3f));
            mySeq.AppendCallback(() => Content.SetActive(true));
            mySeq.Append(Content.GetComponent<Image>().DOFade(1f, 0.3f));



            // mySequence.Play();
        }
        else
        {
            PlaneFilterMat.DOFade(0f, 0.3f);
            Content.GetComponent<Image>().DOFade(0f, 0.3f);
            Content.SetActive(false);
            ContainerRect.DOLocalMoveX(25, 0.3f);

            // mySequence.PlayBackwards();
        }
    }

    private void ToggleImage(float duration = 0.4f)
    {
        if (isOpened)
        {
            ButtonOpen.GetComponent<Image>().DOFade(0f, duration);
            ButtonClosed.GetComponent<Image>().DOFade(1f, duration);
            text.text = "CLOSE";
        }
        else
        {
            ButtonOpen.GetComponent<Image>().DOFade(1f, duration);
            ButtonClosed.GetComponent<Image>().DOFade(0f, duration);
            text.text = "NOTES";
        }
    }
}
