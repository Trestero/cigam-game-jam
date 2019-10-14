using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// VSync-based issues solution found at https://answers.unity.com/questions/587571/standalone-builds-run-very-fast-on-some-computers.html

public class FPSFix : MonoBehaviour
{
    private Resolution res;

    // Start is called before the first frame update
    void Start()
    {
        res = Screen.currentResolution;

        if(res.refreshRate == 60)
        {
            QualitySettings.vSyncCount = 1;
        }
        if (res.refreshRate == 120)
        {
            QualitySettings.vSyncCount = 2;
        }
    }

}
