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
    private LineRenderer lineTargetRenderer;

    public GameObject lineObject;
    public GameObject lineTargetObject;

    private List<Vector2> points = new List<Vector2>();

    private bool isDrawingEnabled = true;

    private List<List<Vector2>> drawings;


    private void Start()
    {
        lineRenderer = lineObject.GetComponent<LineRenderer>();
        lineTargetRenderer = lineTargetObject.GetComponent<LineRenderer>();

        // N shape drawing
        List<Vector2> firstDrawing = new List<Vector2> { Vector2.zero, new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

        // for (int i = 0; i < firstDrawing.Count; i++)
        // {
        //     firstDrawing[i] = firstDrawing[i].normalized;
        // }
        // List<Vector2> secondDrawing = new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero };

        drawings = new List<List<Vector2>> { firstDrawing };

        // drawings[0] = firstDrawing;
        Debug.Log(drawings);

    }

    private void Update()
    {
        AddPoint();

        // UpdateLine(points);

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

                Vector2 vec2HitPoint = new Vector2(hit.point.x, hit.point.y);

                if (points.Count == 0)
                {
                    Debug.Log("adding point");
                    points.Add(vec2HitPoint);
                }
                else if (points[points.Count - 1] != vec2HitPoint)
                {
                    Debug.Log("adding point");
                    points.Add(vec2HitPoint);
                }
            }
        }
        else
        {
            if (points.Count != 0)
            {
                if (isDrawingEnabled)
                {
                    isDrawingEnabled = false;
                    ValidateDrawing();
                    // UnDraw();
                }

            }
        }
    }

    void UpdateLine(List<Vector2> _points)
    {
        lineRenderer.positionCount = _points.Count;

        for (int i = 0; i < _points.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(_points[i].x, _points[i].y, transform.position.z - 0.2f));
        }
    }
    void UpdateLineTarget(List<Vector2> _points)
    {
        lineTargetRenderer.positionCount = _points.Count;

        for (int i = 0; i < _points.Count; i++)
        {
            lineTargetRenderer.SetPosition(i, new Vector3(_points[i].x, _points[i].y, transform.position.z - 0.2f));
        }
    }


    void ValidateDrawing()
    {
        foreach (List<Vector2> drawing in drawings)
        {
            List<Vector2> newUserArray = GetMatchUserArray(drawing);
            float score = GetCompareScore(drawing, newUserArray);
            UpdateLine(newUserArray.ToList());
        }
    }

    List<Vector2> GetMatchUserArray(List<Vector2> drawing)
    {
        Vector2[] newUserArray = new Vector2[drawing.Count];
        newUserArray[0] = points[0];

        for (int i = 1; i < drawing.Count - 1; i++)
        {
            int pointIndex = i * (points.Count / (drawing.Count - 1));
            newUserArray[i] = points[pointIndex];
        }
        newUserArray[^1] = points[points.Count - 1];

        return newUserArray.ToList();
    }

    float GetCompareScore(List<Vector2> drawing, List<Vector2> userArray)
    {
        List<Vector2> drawingCopy = drawing;

        // getting bounding box and scale for user drawing
        Vector2 lowestPoint = new Vector2(userArray[0].x, userArray[0].y);
        Vector2 highestPoint = new Vector2(userArray[0].x, userArray[0].y);

        for (int i = 0; i < userArray.Count; i++)
        {
            if (userArray[i].x < lowestPoint.x) lowestPoint.x = userArray[i].x;
            if (userArray[i].x > highestPoint.x) highestPoint.x = userArray[i].x;
            if (userArray[i].y < lowestPoint.y) lowestPoint.y = userArray[i].y;
            if (userArray[i].y > highestPoint.y) highestPoint.y = userArray[i].y;
        }

        float userDiagonal = GetDiagonal(userArray);
        float drawingDiagonal = GetDiagonal(drawing);

        for (int i = 0; i < drawing.Count; i++)
        {
            drawingCopy[i] *= (userDiagonal / drawingDiagonal);
        }

        // adjusting for user drawing origin
        for (int i = 0; i < drawing.Count; i++)
        {
            drawingCopy[i] += lowestPoint;
        }

        float cumulativeOffsetInc = 0;
        float cumulativeOffsetDec = 0;
        for (int i = 0; i < drawingCopy.Count; i++)
        {
            cumulativeOffsetInc += Vector2.Distance(drawingCopy[i], userArray[i]);
            cumulativeOffsetDec += Vector2.Distance(drawingCopy[i], userArray[^(i + 1)]);
        }

        cumulativeOffsetInc = cumulativeOffsetInc / drawing.Count;
        cumulativeOffsetDec = cumulativeOffsetDec / drawing.Count;

        UpdateLineTarget(drawingCopy);
        return Mathf.Min(cumulativeOffsetDec, cumulativeOffsetInc);
    }


    float GetDiagonal(List<Vector2> drawing)
    {
        float minX = drawing[0].x, maxX = drawing[0].x, minY = drawing[0].y, maxY = drawing[0].y;
        for (int i = 0; i < drawing.Count; i++)
        {
            if (drawing[i].x < minX) minX = drawing[i].x;
            if (drawing[i].x > maxX) maxX = drawing[i].x;
            if (drawing[i].y < minY) minY = drawing[i].y;
            if (drawing[i].y > maxY) maxY = drawing[i].y;
        }

        return Vector2.Distance(new Vector2(minX, minY), new Vector2(maxX, maxY));
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
            // UpdateLine(points);
            lineRenderer.positionCount = points.Count;
            hasRunForIndex[index] = true;
        }
        DOTween.SetTweensCapacity(initialPointsCount, initialPointsCount - 1);
        var tween = DOVirtual.Int(initialPointsCount, 0, 3f, onVirtualUpdate);
        tween.onComplete = () =>
        {
            isDrawingEnabled = true;
        };

    }

}