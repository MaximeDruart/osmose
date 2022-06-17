using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace extOSC.Examples
{
    public class ListenImpulse : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private Material noisewaveMaterial;

        private bool startWave = false;
        private float waveValue = 0f;

        public void SendWave()
        {
            startWave = true;
            waveValue = 0f;
        }

        void FixedUpdate()
        {
            if (startWave)
            {
                waveValue += 0.01f;
                noisewaveMaterial.SetFloat("_uProgress", waveValue);

                waveValue = Mathf.Clamp(waveValue, 0, 1f);
                if (waveValue == 1f)
                {
                    startWave = false;
                    waveValue = 0f;
                }
            }
        }
    }
}
