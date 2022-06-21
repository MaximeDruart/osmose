using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PressionModulesManager : MonoBehaviour
{
    [SerializeField] private float timeToChangeTexture = 10f;

    [SerializeField] private GameObject[] pressionGameObjects = new GameObject[6];
    private Material[] pressionMaterials = new Material[6];
    private bool switchToggle;

    void Start()
    {
        parseTableOfPressionModuleMaterials();
    }


    private void parseTableOfPressionModuleMaterials()
    {
        for (int i = 0; i < pressionGameObjects.Length; i++)
        {
            pressionMaterials[i] = pressionGameObjects[i].GetComponent<Renderer>().material;
        }
    }

    public void TogglePressionModule(int idPressionModuleToToggle, bool isToggle)
    {
        Debug.Log("\nPression Module: " + (idPressionModuleToToggle + 1) + "\nToggle: " + isToggle + "\n Index Pression Module: " + idPressionModuleToToggle);
        if (isToggle)
        {
            pressionMaterials[idPressionModuleToToggle].DOFloat(1, "_Progress_Value", timeToChangeTexture);
        }
        else
        {
            pressionMaterials[idPressionModuleToToggle].DOFloat(0, "_Progress_Value", timeToChangeTexture);
        }
    }
}
