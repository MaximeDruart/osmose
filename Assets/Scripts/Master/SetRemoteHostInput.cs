
using UnityEngine;


public class SetRemoteHostInput : MonoBehaviour
{
    public void setHost(string host)
    {
        Debug.Log(host);
        PortController.Instance.desktopIP = host;
    }

}
