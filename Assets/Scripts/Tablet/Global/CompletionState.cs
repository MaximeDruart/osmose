using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "CompletionState", menuName = "ScriptableObjects/CompletionState", order = 0)]
public class CompletionState : ScriptableObject
{
    public bool EnvironmentCompleted = false;
    public bool ListenCompleted = false;
    public bool MusicCompleted = false;
    public bool DrawingCompleted = false;


    public void SetEnvironmentCompleted()
    {
        EnvironmentCompleted = true;
    }
    public void SetListenCompleted()
    {
        ListenCompleted = true;
    }
    public void SetMusicCompleted()
    {
        MusicCompleted = true;
    }
    public void SetDrawingCompleted()
    {
        DrawingCompleted = true;
    }
}