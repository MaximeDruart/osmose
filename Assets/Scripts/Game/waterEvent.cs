using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterEvent : MonoBehaviour
{

    public GameObject waterObject;
    private Material waterMat;
    // Start is called before the first frame update
    void Start()
    {
        waterMat = waterObject.GetComponent<MeshRenderer>().sharedMaterial;

        waterMat.SetFloat("_Fill", 0.8f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateMat(float fill)
    {
        waterMat = waterObject.GetComponent<MeshRenderer>().sharedMaterial;

        waterMat.SetFloat("_Fill", fill);
    }
}
