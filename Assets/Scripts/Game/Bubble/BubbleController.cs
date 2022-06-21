using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float timeSpan = 1f;
    [SerializeField] private float xRangeSpawnStart = -5f;
    [SerializeField] private float xRangeSpawnEnd = 5f;
    [SerializeField] private float zRangeSpawnStart = -5f;
    [SerializeField] private float zRangeSpawnEnd = 5f;
    [SerializeField] private bool isVariation = false;


    void Start()
    {
        StartCoroutine(Bubbling());
    }

    void Update()
    {

    }

    private IEnumerator Bubbling()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeSpan);

            if (isVariation)
            {
                zRangeSpawnEnd = Random.Range(zRangeSpawnEnd - 0.04f, zRangeSpawnEnd + 0.04f);
            }

            GameObject bubbleInstance = (GameObject)Instantiate(bubblePrefab);
            bubbleInstance.transform.position = new Vector3(Random.Range(xRangeSpawnStart, xRangeSpawnEnd), 0, Random.Range(zRangeSpawnStart, zRangeSpawnEnd));
        }
    }
}
