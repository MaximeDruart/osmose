using UnityEngine;
using UnityEngine.Events;

public class ReceiveListen : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] private Material noisewaveMaterial
    void Start()
    {
        // noisewaveMaterial.SetFloat("_uWaveOpacity", 0);
    }


    // ADRESS : /listen/frequency
    public void ReceiveFreq(float freq)
    {
        // noisewaveMaterial.SetFloat("_uWaveOpacity", opacity);

        // adjust the wave opacity between 0 and bototm threshold
        // not sure if we still want it

    }

    // ADRESS : /listen/frequency/completed
    public void ReceiveFreqCompleted()
    {
        // make the wave easy to see on the creature

    }

    // ADRESS : /listen/color
    public void ReceiveColor(Color color)
    {
        // adjust the lights and fog according to this color
    }

}
