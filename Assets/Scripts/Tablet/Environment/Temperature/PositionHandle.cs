using UnityEngine;

public class PositionHandle : MonoBehaviour
{
    // Start is called before the first frame update

    public float radius;

    private RectTransform rect;

    private float fullRotationDeg = 270;


    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();

        SetHandlePosition(0);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static Vector2 PolarToCartesian(float angle, float radius)
    {
        float angleRad = (Mathf.PI / 180.0f) * (angle - 90f);
        float x = radius * Mathf.Cos(angleRad);
        float y = radius * Mathf.Sin(angleRad);
        return new Vector2(x, y);
    }


    public void SetHandlePosition(float value)
    {
        float halfBottomAngle = (360 - fullRotationDeg) / 2;

        float angle = extOSC.OSCUtilities.Map(value, 0, 1, 360 - halfBottomAngle, halfBottomAngle);

        float rotation = extOSC.OSCUtilities.Map(value, 0, 1, 180 - (halfBottomAngle), -(180 - (halfBottomAngle)));

        Vector2 newPos = PolarToCartesian(angle, radius);
        rect.anchoredPosition = newPos;

        rect.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}
