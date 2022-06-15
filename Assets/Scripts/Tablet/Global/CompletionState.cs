using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "CompletionState", menuName = "ScriptableObjects/CompletionState", order = 0)]
public class CompletionState : ScriptableObject
{

    public Dictionary<string, bool> completedModules;

    private void Awake()
    {
        completedModules.Add("Environment", false);
        completedModules.Add("Listen", false);
        completedModules.Add("Music", false);
        completedModules.Add("Drawing", false);
    }

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
}