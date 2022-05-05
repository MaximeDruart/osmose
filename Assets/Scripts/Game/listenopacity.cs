using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class listenopacity : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Material noisewaveMaterial;
    void Start()
    {
        noisewaveMaterial.SetFloat("_uWaveOpacity", 0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateOpacity(float opacity)
    {
        noisewaveMaterial.SetFloat("_uWaveOpacity", opacity);

    }
}
