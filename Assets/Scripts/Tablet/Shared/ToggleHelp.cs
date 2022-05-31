using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleHelp : MonoBehaviour
{

    private bool isOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle(bool _) {
        isOpened = !isOpened;
    }
}
