using UnityEngine;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class ValidateTempPressure : MonoBehaviour {


    [SerializeField] private GameObject CreatureObj;
    // private Material CreatureMat;

    private float startX = -636f;
    private float endX = -637.907f;



    private void Start() {
        // CreatureMat = CreatureObj.GetComponent<Renderer>().material;
        // CreatureMat.SetFloat("_uOpacity", 0.6f);
        // CreatureObj.transform.DOMoveX(startX, 0.001f);

    }

    public void onValidTempPressure() {
        Sequence mySequence = DOTween.Sequence();
        // mySequence.Append(CreatureObj.transform.DOMoveX(endX, 1f));
        // CreatureMat.SetFloat("_uOpacity", 1f);
    }
}