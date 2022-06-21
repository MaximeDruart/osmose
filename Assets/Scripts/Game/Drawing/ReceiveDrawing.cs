using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        public GameObject Canvas;



        [SerializeField] private float animationDuration = 2f;

        private LineRenderer lineRenderer;
        private Vector3[] linePoints;

        private List<Vector2> points = new List<Vector2>();

        private int activeDrawing = 0;

        public GameObject Mask;
        private RectTransform MaskRect;
        private RectMask2D MaskMask;

        public GameObject DrawingContainer;
        private Image DotBackground;


        [SerializeField] private GameObject SymbolObject;
        private Material SymbolMat;
        private Color SymbolMatStartColor;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Mesh skinnedMesh;


        public GameObject[] emissionObjects;
        private Material[] emissionMaterials = new Material[4];



        public UnityEvent OnCompleted;


        // Start is called before the first frame update
        void Start()
        {
            lineRenderer = LineObject.GetComponent<LineRenderer>();

            DotBackground = DrawingContainer.GetComponent<Image>();
            DotBackground.color = new Color32(160, 197, 255, 0);

            MaskRect = Mask.GetComponent<RectTransform>();
            MaskMask = Mask.GetComponent<RectMask2D>();
            SetBorderPadding(8);

            for (int i = 0; i < emissionObjects.Length; i++)
            {
                emissionMaterials[i] = emissionObjects[i].GetComponent<Renderer>().material;
            }

            skinnedMeshRenderer = SymbolObject.GetComponent<SkinnedMeshRenderer>();
            skinnedMesh = SymbolObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            SymbolMat = SymbolObject.GetComponent<SkinnedMeshRenderer>().material;

            SymbolMatStartColor = SymbolMat.GetColor("_EmissionColor");
            SymbolMat.SetColor("_EmissionColor", new Color(0, 0, 0, 0));

            Receiver.Bind(Address, ReceiveMessage);
        }

        void IncrementActiveDrawing()
        {
            if (activeDrawing < 3)
            {
                Debug.Log("good rawing");
                activeDrawing++;
            }
        }


        void ReceiveMessage(OSCMessage message)
        {
            if (message.ToArray(out var arrayValues)) // Get all values from first array in message.
            {
                points.Clear();
                if (arrayValues[0].BoolValue) IncrementActiveDrawing();

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

        void SetBorderPadding(float pos)
        {
            MaskMask.padding = new Vector4(pos, 0, pos, 0);
        }



        void OnReceive(bool isValid)
        {
            lineRenderer.positionCount = points.Count;
            lineRenderer.material.DOFade(1, 0f);
            Sequence mySequence = DOTween.Sequence();
            DOVirtual.Float(8, 0, 0.9f, SetBorderPadding).SetEase(Ease.OutQuint);
            mySequence.AppendInterval(0.8f);
            mySequence.Append(DotBackground.DOColor(new Color32(160, 197, 255, 103), 0.6f));
            mySequence.AppendCallback(() => StartCoroutine(AnimateLine()));
            if (isValid)
            {
                mySequence.AppendCallback(UpdateCreatureDrawing);
            }
            mySequence.AppendInterval(6f);
            mySequence.Append(DotBackground.DOColor(new Color32(160, 197, 255, 0), 1f));
            mySequence.AppendCallback(() =>
            {
                DOVirtual.Float(0, 8, 0.9f, SetBorderPadding).SetEase(Ease.OutQuint);
            });
            mySequence.AppendInterval(0.5f);
            mySequence.Append(lineRenderer.material.DOFade(0, 0.5f));
            mySequence.AppendCallback(ClearDrawing);
            mySequence.AppendCallback(CheckForValidation);

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
                SetGlowIntensity(0.3f);
                DOVirtual.Int(0, 100, 2, (int i) =>
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(0, i);
                });
            }
            if (activeDrawing == 2)
            {
                SetGlowIntensity(0.6f);
                DOVirtual.Int(100, 0, 2, (int i) =>
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(0, i);
                });
                DOVirtual.Int(0, 100, 2, (int i) =>
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(1, i);
                });
            }
            if (activeDrawing == 3)
            {
                SetGlowIntensity(1f);
                SymbolMat.DOFade(0, 1.5f);
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

        private void SetGlowIntensity(float intensity)
        {
            foreach (var mat in emissionMaterials)
            {
                mat.DOFloat(intensity, "_EmissionMapIntensity", 1f);
                mat.DOFloat(intensity, "_EmissionZoneIntensity", 1f);
            }

        }

        public void ShowDrawing()
        {
            SymbolMat.DOColor(SymbolMatStartColor, "_EmissionColor", 2.5f);
        }

    }
}
