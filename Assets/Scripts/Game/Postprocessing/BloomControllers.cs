using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomControllers : MonoBehaviour
{
    [SerializeField] private UnityEngine.Rendering.Volume volume;
    private Bloom b;


    void Start()
    {
        volume.profile.TryGet(out b);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            b.tint.value = new Color(UnityEngine.Random.Range(1.1f, 10.9f), UnityEngine.Random.Range(1.1f, 10.9f), UnityEngine.Random.Range(1.1f, 10.9f), 1.0F);
        }
    }

    public void SetBloomColor(Color color)
    {
        Color colorTemp = b.tint.value;
        DOVirtual.Color(b.tint.value, color, 0.6f, (Color c) =>
        {
            b.tint.value = c;
        });

    }
}
