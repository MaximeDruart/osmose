using UnityEngine;

public class ResetCompletionState : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private CompletionState completionState;
    [SerializeField] private bool doReset = true;

    void Start()
    {
        if (doReset) completionState.ResetState();
    }
}
