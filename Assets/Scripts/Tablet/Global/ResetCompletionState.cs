using UnityEngine;

public class ResetCompletionState : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private CompletionState completionState;
    [SerializeField] private bool doReset = true;

    void Awake()
    {
        if (doReset)
        {
            completionState.EnvironmentCompleted = false;
            completionState.ListenCompleted = false;
            completionState.MusicCompleted = false;
            completionState.DrawingCompleted = false;
        }

    }
}
