
using UnityEngine;


public class SetRemoteHostInput : MonoBehaviour
{
    public string defaultHost = "10.192.3.177";


    private void Start()
    {
        PortController.Instance.remoteIp = defaultHost;

    }

    public void setHost(string host)
    {
        Debug.Log(host);
        PortController.Instance.remoteIp = host;
    }

}
