using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace extOSC
{
    public class Draw : MonoBehaviour
    {

        public OSCTransmitter Transmitter;
        public string BaseAddress = "/draw";

        public GameObject Brush;
        public float brushSize = 3f;

        private LineRenderer lineRenderer;
        private LineRenderer lineTargetRenderer;

        public GameObject lineObject;
        public GameObject lineTargetObject;

        private List<Vector2> points = new List<Vector2>();

        private bool isDrawingEnabled = true;

        private List<List<Vector2>> drawings;

        private float[] validationThresholds = { 0.25f };

        private int activeDrawing = 0;


        private void Start()
        {
            lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineTargetRenderer = lineTargetObject.GetComponent<LineRenderer>();

            // N shape drawing
            List<Vector2> firstDrawing = new List<Vector2> {
                new Vector2(1,4),
                new Vector2(2,5),
                new Vector2(2.4f,4),
                new Vector2(2,3.3f),
                new Vector2(1.7f,3),
                new Vector2(1,2.2f),
                new Vector2(0.0f,0.6f),
                new Vector2(0.5f,0),
                new Vector2(1,0.4f),
                new Vector2(1.5f,1),
                new Vector2(2,1.6f),
                new Vector2(3,2),
                new Vector2(4,1.5f),
                new Vector2(4.5f,1),
                new Vector2(5,0.3f),
                new Vector2(5.8f,1),
                new Vector2(5,2),
                new Vector2(4.2f,3),
                new Vector2(3.5f,4),
                new Vector2(4,5),
                new Vector2(5,4)
            };

            // List<Vector2> secondDrawing = new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero };


            drawings = new List<List<Vector2>> { firstDrawing };
        }

        private void Update()
        {
            AddPoint();
            UpdateLine(points);

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
                        points.Add(vec2HitPoint);
                    }
                    else if (points[points.Count - 1] != vec2HitPoint)
                    {
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
                        bool drawingIsValid = isDrawingValid(drawings[activeDrawing]);
                        Debug.Log(drawingIsValid);


                        //SendDrawing(points, drawingIsValid);
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


        bool isDrawingValid(List<Vector2> drawing)
        {
            List<Vector2> newUserArray = GetMatchUserArray(drawing);
            float score = GetCompareScore(drawing, newUserArray);
            Debug.Log(score);
            return score < validationThresholds[activeDrawing];
        }


        void ValidateDrawing()
        {


        }

        void SendDrawing(List<Vector2> _points, bool isValid)
        {
            OSCMessage message = new OSCMessage(BaseAddress);

            var array = OSCValue.Array();
            array.AddValue(OSCValue.Bool(isValid));
            foreach (Vector2 point in _points)
            {
                array.AddValue(OSCValue.Float(point.x));
                array.AddValue(OSCValue.Float(point.y));
            }
            message.AddValue(array);
            Transmitter.Send(message);
        }

        // Reduces the user array to have the same amount of points as the stored drawing
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
            // we first need to make sure the comparison condition are good : the origin of both drawing, their scale too, also allow the user to draw it in reverse
            List<Vector2> drawingCopy = drawing.ConvertAll(vec => new Vector2(vec.x, vec.y));

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

            for (int i = 0; i < drawingCopy.Count; i++)
            {
                drawingCopy[i] *= (userDiagonal / drawingDiagonal);
            }

            // adjusting for user drawing origin
            for (int i = 0; i < drawingCopy.Count; i++)
            {
                drawingCopy[i] += lowestPoint;
            }

            // generating the score as the total of the distance between each points of the same index of each array
            float cumulativeOffsetInc = 0;
            float cumulativeOffsetDec = 0;
            for (int i = 0; i < drawingCopy.Count; i++)
            {
                cumulativeOffsetInc += Vector2.Distance(drawingCopy[i], userArray[i]);
                cumulativeOffsetDec += Vector2.Distance(drawingCopy[i], userArray[^(i + 1)]);
            }

            float offset = Mathf.Min(cumulativeOffsetDec, cumulativeOffsetInc);

            // make it relative to how many points we're matching with
            // aims to make it easier to match large ensemble of points
            offset /= drawing.Count;

            // remove the scale component : otherwise bigger scale would induce a smaller possible error margin for the drawing
            offset /= userDiagonal;

            UpdateLineTarget(drawingCopy);
            return offset;
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
            Debug.Log("undrawing");
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
}