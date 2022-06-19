using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CheckForEndGame : MonoBehaviour
{
    // Start is called before the first frame update
    public CompletionState completionState;
    public UnityEvent OnGameCompletion;

    public void CheckForEnd()
    {
        if (completionState.IsCompleted())
        {

            Sequence mySequence = DOTween.Sequence();

            // creature goes to zap
            // lightning effect
            // end music
            // reset on tl completion (will probably come from the tablet)

            mySequence.AppendCallback(OnGameCompletion.Invoke);
        }
    }
}
