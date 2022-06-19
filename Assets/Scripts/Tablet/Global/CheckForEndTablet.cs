using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CheckForEndTablet : MonoBehaviour
{
    public CompletionState completionState;
    public UnityEvent OnTabletCompletion;

    public void CheckForEnd()
    {
        if (completionState.IsCompleted())
        {

            Sequence mySequence = DOTween.Sequence();

            // fade to black ?
            // wait for the end of game animation
            // trigger reset for both sides

            mySequence.AppendCallback(OnTabletCompletion.Invoke);
        }
    }
}
