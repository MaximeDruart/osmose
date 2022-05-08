
using UnityEngine;


public class SetDeviceIp : MonoBehaviour
{
    public string defaultIp = "10.192.3.177";


    private void Start()
    {
        PortController.Instance.deviceIp = defaultIp;

    }

    public void setIp(string ip)
    {
        PortController.Instance.deviceIp = ip;
    }

}
