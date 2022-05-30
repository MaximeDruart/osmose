using UnityEngine;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ReceiveDrawing : MonoBehaviour {


    [SerializeField] private GameObject CreatureObj;
    private Material CreatureMat;


    // private void Start() {
    //     CreatureMat = CreatureObj.GetComponent<Renderer>().material;
    //     CreatureMat.setFloat("_uOpacity", 0.6);
    // }

    // public void onValidTempPressure() {
    //     Sequence mySequence = DOTween.Sequence();
    //     mySequence.Append(CreatureObj.DOMoveX(x, 1));
    //     CreatureMat.setFloat("_uOpacity", 1);
    // }
}