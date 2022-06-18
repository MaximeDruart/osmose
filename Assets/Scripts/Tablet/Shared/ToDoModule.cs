using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToDoModule : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool isDone = false;

    [SerializeField] private GameObject innerCircle;

    private Material innerCircleMaterial;


    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject Border;

    [SerializeField] private ColorVariable colorVariable;

    private Image BorderImage;



    void Start()
    {
        innerCircleMaterial = innerCircle.GetComponent<Renderer>().material;
        BorderImage = Border.GetComponent<Image>();
    }

    public void SetIsDone(bool _isDone)
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(innerCircleMaterial.DOFade(0.8f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(0.3f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(0.8f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(0.3f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(1f, 0.5f));
        mySequence.Join(innerCircleMaterial.DOColor(colorVariable.color, 0.5f));
        mySequence.Join(BorderImage.DOColor(colorVariable.color, 0.5f));
        mySequence.Join(text.DOColor(colorVariable.color, 0.5f));
        mySequence.Join(text.DOText("MODULE COMPLETE", 0.5f));
        mySequence.AppendCallback(() =>
        {
            isDone = _isDone;
        });

    }

}
