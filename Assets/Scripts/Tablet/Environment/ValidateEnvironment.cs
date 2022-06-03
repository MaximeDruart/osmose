using UnityEngine;
using UnityEngine.Events;



public class ValidateEnvironment : MonoBehaviour
{
    // Start is called before the first frame update


    public UnityEvent onValidation;

    private bool isTempValidated = false;
    private bool isPressureValidated = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ValidateTemperature(bool isValidated)
    {
        isTempValidated = isValidated;
        Validate();
    }
    public void ValidatePressure(bool isValidated)
    {
        isPressureValidated = isValidated;
        Validate();
    }


    private void Validate()
    {
        if (isTempValidated && isPressureValidated) onValidation.Invoke();
    }
}
