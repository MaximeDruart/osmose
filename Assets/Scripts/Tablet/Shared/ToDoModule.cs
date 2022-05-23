using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToDoModule : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] bool isDone = false;

    [SerializeField] GameObject innerCircle;

    private Material innerCircleMaterial;

    void Start()
    {
        innerCircleMaterial = innerCircle.GetComponent<Renderer>().material;
    }

    public void SetIsDone(bool _isDone)
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(innerCircleMaterial.DOFade(0.8f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(0.3f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(0.8f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(0.3f, 0.5f));
        mySequence.Append(innerCircleMaterial.DOFade(0.8f, 0.5f));


        mySequence.AppendCallback(() =>
        {
            isDone = _isDone;
        });

    }
}
