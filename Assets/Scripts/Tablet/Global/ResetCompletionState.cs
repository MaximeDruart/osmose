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
            completionState.completedModules["Environment"] = false;
            completionState.completedModules["Listen"] = false;
            completionState.completedModules["Music"] = false;
            completionState.completedModules["Drawing"] = false;

        }

    }
}
