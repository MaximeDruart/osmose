using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



namespace extOSC
{
    public class ReceiveDrawing : MonoBehaviour
    {

        [Header("extOSC settings")]
        [SerializeField]
        OSCReceiver Receiver;

        [SerializeField]
        string Address = "/draw";


        [Header("Drawing settings")]
        public GameObject Brush;
        public GameObject LineObject;

        public GameObject Loader;


        public GameObject Canvas;



        [SerializeField] private float animationDuration = 2f;

        private LineRenderer lineRenderer;
        private Vector3[] linePoints;

        private List<Vector2> points = new List<Vector2>();

        private int activeDrawing = 0;


        [SerializeField] private TMP_Text DrawingCounterText;

        private int drawingCounter = 0;

        public GameObject DrawingContainer;
        private Image DotBackground;


        [SerializeField] private GameObject SymbolObject;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Mesh skinnedMesh;

        public UnityEvent OnCompleted;


        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = LineObject.GetComponent<LineRenderer>();
            DotBackground = DrawingContainer.GetComponent<Image>();

            Receiver.Bind(Address, ReceiveMessage);

            Canvas.transform.localScale = Vector3.zero;
            DotBackground.color = new Color32(160, 197, 255, 0);
            Loader.transform.localScale = Vector3.zero;

            skinnedMeshRenderer = SymbolObject.GetComponent<SkinnedMeshRenderer>();
            skinnedMesh = SymbolObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        }

        void onValidDrawing()
        {
            if (activeDrawing < 3)
            {
                activeDrawing++;
            }
        }


        void ReceiveMessage(OSCMessage message)
        {
            if (message.ToArray(out var arrayValues)) // Get all values from first array in message.
            {
                points.Clear();
                if (arrayValues[0].BoolValue) onValidDrawing();

                // we're sending in a 1D array so we need to split it to get an array of vector2s
                // we're scrolling the array 2 by 2 and creating a vector with the current value and the next
                // 1 4 2 3 2 4
                // -> vec2(1, 4), vec2(2, 3), vec2(2, 4),

                for (int i = 1; i < arrayValues.Count; i += 2)
                {
                    Vector2 newVec = new Vector2(arrayValues[i].FloatValue, arrayValues[i + 1].FloatValue);
                    points.Add(newVec);
                }
                OnReceive(arrayValues[0].BoolValue);
            }

        }

        void IncrementText()
        {
            int incCounter = drawingCounter + 1;
            DrawingCounterText.text = "00" + incCounter.ToString();

        }

        void OnReceive(bool isValid)
        {
            lineRenderer.positionCount = points.Count;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(Canvas.transform.DOScale(Vector3.one * 0.15f, 0.3f));
            mySequence.Append(DotBackground.DOColor(new Color32(160, 197, 255, 103), 0.6f));
            mySequence.AppendCallback(() => StartCoroutine(AnimateLine()));
            var tween = mySequence.Append(Loader.transform.DOScale(Vector3.one * 50f, 0.3f))
            .OnStart(() => Loader.transform.DOLocalRotate(new Vector3(0, 600, 0), 6.6f).SetEase(Ease.Linear));

            if (isValid)
            {
                mySequence.AppendCallback(UpdateCreatureDrawing);
            }
            mySequence.AppendInterval(6f);
            mySequence.Append(Loader.transform.DOScale(Vector3.zero, 0.3f));
            mySequence.Append(DotBackground.DOColor(new Color32(160, 197, 255, 0), 1f));
            mySequence.Append(Canvas.transform.DOScale(Vector3.zero, 1f));
            mySequence.AppendCallback(ClearDrawing);
            mySequence.AppendCallback(CheckForValidation);
            IncrementText();

        }

        void ClearDrawing()
        {
            lineRenderer.positionCount = 0;
        }

        void CheckForValidation()
        {
            if (activeDrawing == 3)
            {
                OnCompleted.Invoke();
            }
        }

        void UpdateCreatureDrawing()
        {
            if (activeDrawing == 1)
            {
                DOVirtual.Int(0, 100, 2, (int i) =>
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(0, i);
                });
            }
            if (activeDrawing == 2)
            {
                DOVirtual.Int(100, 0, 2, (int i) =>
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(0, i);
                });
                DOVirtual.Int(0, 100, 2, (int i) =>
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(1, i);
                });
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


        private IEnumerator AnimateLine()
        {
            float segmentDuration = animationDuration / points.Count;
            lineRenderer.SetPosition(0, points[0]);


            for (int i = 0; i < points.Count - 1; i++)
            {
                float startTime = Time.time;

                Vector3 startPosition = points[i];
                Vector3 endPosition = points[i + 1];

                Vector3 pos = startPosition;
                while (pos != endPosition)
                {
                    float t = (Time.time - startTime) / segmentDuration;
                    pos = Vector3.Lerp(startPosition, endPosition, t);

                    // animate all other points except point at index i
                    for (int j = i + 1; j < points.Count; j++)
                        lineRenderer.SetPosition(j, pos);

                    yield return null;
                }
            }
        }
    }
}
