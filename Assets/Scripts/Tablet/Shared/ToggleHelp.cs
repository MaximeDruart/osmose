using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToggleHelp : MonoBehaviour
{

    private bool isOpened = false;

    public GameObject Sidebar;
    private RectTransform SidebarRect;

    private Sequence mySequence;
    // Start is called before the first frame update
    void Start()
    {
        SidebarRect = Sidebar.GetComponent<RectTransform>();
        mySequence = DOTween.Sequence();
        mySequence.Append(SidebarRect.DOLocalMoveX(120, 0.3f));
        mySequence.Pause();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle(bool _)
    {
        isOpened = !isOpened;
        if (!isOpened)
        {
            Debug.Log("playing !!");
            mySequence.Play();
        }
        else
        {
            mySequence.PlayBackwards();
        }
    }
}
