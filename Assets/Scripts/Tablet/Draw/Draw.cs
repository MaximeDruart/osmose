using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace extOSC
{
    public class Draw : MonoBehaviour
    {

        [Header("extOSC")]

        public OSCTransmitter Transmitter;
        public string BaseAddress = "/draw";

        [Header("Drawing settings")]
        public GameObject Brush;
        public GameObject lineObject;

        [Header("Match preview")]
        public GameObject lineTargetObject;
        public bool showPreview = false;

        [Space(10)]

        public UnityEvent<bool> onCompleted;
        public BoolVariable HelpIsOpened;


        private LineRenderer lineRenderer;
        private LineRenderer lineTargetRenderer;

        private List<Vector2> points = new List<Vector2>();

        private bool isDrawingEnabled = true;


        private List<List<Vector2>> drawings;

        private float[] validationThresholds = { 0.25f, 0.25f, 0.25f };

        private int activeDrawing = 0;

        private AudioSource audioSource;

        [SerializeField]
        private GameObject ProgressionBar;

        [SerializeField]
        private GameObject ProgressionContainer;

        private RectTransform ProgressionContainerRect;
        private RectTransform ProgressionBarRect;


        private void Start()
        {
            lineRenderer = lineObject.GetComponent<LineRenderer>();
            lineTargetRenderer = lineTargetObject.GetComponent<LineRenderer>();

            audioSource = GetComponent<AudioSource>();

            ProgressionContainerRect = ProgressionContainer.GetComponent<RectTransform>();
            ProgressionBarRect = ProgressionBar.GetComponent<RectTransform>();


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

            List<Vector2> secondDrawing = new List<Vector2> {
                new Vector2(0,1.7f),
                new Vector2(0.5f,1),
                new Vector2(1.5f,0),
                new Vector2(1.5f,1),
                new Vector2(0.9f,2),
                new Vector2(0.5f,3),
                new Vector2(1,3.5f),
                new Vector2(1.8f,3),
                new Vector2(2.8f,3),
                new Vector2(2,4.2f),
                new Vector2(3.4f,5),
                new Vector2(4,4.7f),
                new Vector2(4.5f,4),
                new Vector2(3.8f,3),
                new Vector2(4.6f,3),
                new Vector2(6,3.2f),
                new Vector2(5.5f,2),
                new Vector2(5,1),
                new Vector2(5,0),
                new Vector2(6,1),
                new Vector2(6.5f,1.7f),
            };

            List<Vector2> thirdDrawing = new List<Vector2> {
                new Vector2(4.5f,2),
                new Vector2(3,5),
                new Vector2(3,3.8f),
                new Vector2(2.2f,3),
                new Vector2(1.2f,2),
                new Vector2(0,2.5f),
                new Vector2(1,3.5f),
                new Vector2(2,3),
                new Vector2(2.5f,2),
                new Vector2(3,1),
                new Vector2(3.8f,0),
                new Vector2(4.5f,1),
                new Vector2(5,2),
                new Vector2(5.5f,3),
                new Vector2(7,3.5f),
                new Vector2(7,2),
                new Vector2(6,2),
                new Vector2(5,3),
                new Vector2(4.2f,4),
                new Vector2(5,5),
                new Vector2(5.8f,4.2f),
            };

            drawings = new List<List<Vector2>> { firstDrawing, secondDrawing, thirdDrawing };
        }

        private void Update()
        {
            AddPoint();
            UpdateLine(points);

        }


        void AddPoint()
        {
            if (Input.GetMouseButton(0))
            {
                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(Ray, out hit))
                {
                    if (hit.transform.name != "DrawingBoard") return;
                    if (!isDrawingEnabled) return;
                    if (HelpIsOpened.Value) return;

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
                        ToggleLoading(true);


                        bool drawingIsValid = isDrawingValid(drawings[activeDrawing]);
                        SendDrawing(GetNormalizedPointsToSend(points), drawingIsValid);
                        if (drawingIsValid) activeDrawing++;


                        StartCoroutine(UnDraw(3));


                        if (activeDrawing == drawings.Count)
                        {
                            isDrawingEnabled = false;
                            TriggerCompleted();
                        }
                    }

                }
            }
        }

        List<Vector2> GetNormalizedPointsToSend(List<Vector2> _points)
        {

            float scale = 5f;

            List<Vector2> drawingCopy = _points.ConvertAll(vec => new Vector2(vec.x, vec.y));

            // making sure the first point is in 0/0

            Vector2 lowestPoint = new Vector2(drawingCopy[0].x, drawingCopy[0].y);
            Vector2 highestPoint = new Vector2(drawingCopy[0].x, drawingCopy[0].y);

            for (int i = 0; i < drawingCopy.Count; i++)
            {
                if (drawingCopy[i].x < lowestPoint.x) lowestPoint.x = drawingCopy[i].x;
                if (drawingCopy[i].x > highestPoint.x) highestPoint.x = drawingCopy[i].x;
                if (drawingCopy[i].y < lowestPoint.y) lowestPoint.y = drawingCopy[i].y;
                if (drawingCopy[i].y > highestPoint.y) highestPoint.y = drawingCopy[i].y;
            }

            for (int i = 0; i < drawingCopy.Count; i++)
            {
                drawingCopy[i] = points[i] - lowestPoint;
            }


            float diagonal = GetDiagonal(_points);


            for (int i = 0; i < drawingCopy.Count; i++)
            {
                drawingCopy[i] *= (scale / diagonal);
            }


            return drawingCopy;
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

        void TriggerCompleted()
        {
            onCompleted.Invoke(true);
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

        bool isDrawingValid(List<Vector2> drawing)
        {
            List<Vector2> newUserArray = GetMatchUserArray(drawing);
            float score = GetCompareScore(drawing, newUserArray);
            return score < validationThresholds[activeDrawing];
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

            if (showPreview) UpdateLineTarget(drawingCopy);
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

        IEnumerator UnDraw(float delay)
        {
            yield return new WaitForSeconds(delay);

            points.Clear();
            UpdateLine(new List<Vector2>());
            UpdateLineTarget(new List<Vector2>());

            ToggleLoading(false);
            if (drawings.Count != activeDrawing)
            {
                isDrawingEnabled = true;
            }

        }

        void ToggleLoading(bool isTrue)
        {
            if (isTrue)
            {
                PlaySound();
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(
                ProgressionBarRect.DOSizeDelta(new Vector2(ProgressionContainerRect.rect.width, ProgressionBarRect.rect.height), 1f)
                );
                mySequence.Join(lineRenderer.material.DOFade(0, 0.7f));
                mySequence.Append(ProgressionBar.GetComponent<Image>().DOFade(0f, 0.6f));
                mySequence.Append(
                ProgressionBarRect.DOSizeDelta(new Vector2(0, ProgressionBarRect.rect.height), 0f)
                );
            }
            else
            {
                ProgressionBar.GetComponent<Image>().DOFade(1f, 0f);
                lineRenderer.material.DOFade(1, 0.3f);
            }
        }

        void PlaySound()
        {
            audioSource.Play();
        }
    }

}