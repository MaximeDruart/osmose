using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Draw : MonoBehaviour
{
    public GameObject Brush;
    public float brushSize = 3f;

    private LineRenderer lineRenderer;

    public GameObject lineObject;

    private List<Vector3> points = new List<Vector3>();

    private bool isDrawingEnabled = true;


    private void Start()
    {
        lineRenderer = lineObject.GetComponent<LineRenderer>();

    }

    private void Update()
    {
        AddPoint();
        UpdateLine();
    }

    void Drawing()
    {
        if (Input.GetMouseButton(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                if (hit.transform.name != "drawingboard") return;


                var go = Instantiate(Brush, hit.point, Quaternion.identity, transform);

                go.layer = LayerMask.NameToLayer("Ignore Raycast");

                // go.transform.localScale = Vector3.one * brushSize;
                go.transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
        }


    }

    void AddPoint()
    {
        if (Input.GetMouseButton(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                if (hit.transform.name != "drawingboard") return;
                if (!isDrawingEnabled) return;

                if (points.Count == 0)
                {
                    points.Add(hit.point);
                    lineRenderer.positionCount = points.Count;
                }
                else if (points[points.Count - 1] != hit.point)
                {
                    points.Add(hit.point);
                    lineRenderer.positionCount = points.Count;
                }
            }
        }
        else
        {
            if (points.Count != 0)
            {
                if (isDrawingEnabled)
                {
                    UnDraw();
                    isDrawingEnabled = false;
                }

            }
        }
    }

    void UpdateLine()
    {
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, transform.position.z - 0.2f));
        }
    }

    void UnDraw()
    {
        Debug.Log("running undraw !");
        int initialPointsCount = points.Count;
        bool[] hasRunForIndex = new bool[points.Count];
        for (int i = 0; i < hasRunForIndex.Length; i++)
            hasRunForIndex[i] = false;

        void onVirtualUpdate(int index)
        {
            if (hasRunForIndex[index]) return;
            points.RemoveAt(index);
            UpdateLine();
            lineRenderer.positionCount = points.Count;
            hasRunForIndex[index] = true;
        }
        DOTween.SetTweensCapacity(initialPointsCount, initialPointsCount - 1);
        var tween = DOVirtual.Int(initialPointsCount, 0, 3f, onVirtualUpdate);
        tween.onComplete = () => isDrawingEnabled = true;
    }

}