using UnityEngine;
using UnityEngine.Rendering;

public class URPDebugger : MonoBehaviour
{
    private void Awake()
    {
        DebugManager.instance.enableRuntimeUI = false;
    }
}