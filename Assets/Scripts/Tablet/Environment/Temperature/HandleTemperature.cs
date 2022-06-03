
using UnityEngine;
using UnityEngine.Events;

public class HandleTemperature : MonoBehaviour
{

    public float targetValue = 0.51f;
    public float validationRange = 0.1f;

    public UnityEvent<bool> validateTemperature;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkForValidation(float value)
    {
        validateTemperature.Invoke(value > targetValue - validationRange && value < targetValue + validationRange);
    }
}
