using DG.Tweening;
using UnityEngine;

public class EyesBlink : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject EyesObject;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh skinnedMesh;
    private Material eyeMaterial;


    public float eyeBlinkDelay = 400;

    public CompletionState completionState;

    private float delay = 0;

    void Start()
    {
        skinnedMeshRenderer = EyesObject.GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = EyesObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        eyeMaterial = EyesObject.GetComponent<SkinnedMeshRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (completionState.completedModules["Environment"]) delay += 1;

        if (delay >= eyeBlinkDelay)
        {
            delay = 0;
            AnimateEyes();
        }
    }

    private void AnimateEyes()
    {
        float currentValue = skinnedMeshRenderer.GetBlendShapeWeight(0);
        int currentValueInt = Mathf.FloorToInt(currentValue);
        DOVirtual
            .Int(0, 100, 0.4f,
            (int i) => skinnedMeshRenderer.SetBlendShapeWeight(0, i)
            );
        DOVirtual
            .Int(100, 0, 0.4f,
            (int i) => skinnedMeshRenderer.SetBlendShapeWeight(0, i)
            ).SetDelay(0.45f);

    }

}
