using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[CreateAssetMenu(fileName = "CompletionState", menuName = "ScriptableObjects/CompletionState", order = 0)]
public class CompletionState : ScriptableObject
{

    public Dictionary<string, bool> completedModules = new Dictionary<string, bool>(){
    {"Environment", false},
    {"Listen", false},
    {"Music", false},
    {"Drawing", false},
};

    public bool isTablet = true;

    public UnityEvent onTabletEnd;
    public UnityEvent onGameEnd;

    public void SetEnvironmentCompleted()
    {
        completedModules["Environment"] = true;

    }
    public void SetListenCompleted()
    {
        completedModules["Listen"] = true;

    }
    public void SetMusicCompleted()
    {
        completedModules["Music"] = true;

    }
    public void SetDrawingCompleted()
    {
        completedModules["Drawing"] = true;

    }
    public int getNoOfCompletedModules()
    {
        int c = 0;
        foreach (KeyValuePair<string, bool> module in completedModules)
        {
            if (module.Value) c++;
        }

        return c;

    }

    public void ResetState()
    {
        completedModules["Environment"] = false;
        completedModules["Listen"] = false;
        completedModules["Music"] = false;
        completedModules["Drawing"] = false;
    }

    public bool IsCompleted()
    {
        return getNoOfCompletedModules() == 4;
    }

}