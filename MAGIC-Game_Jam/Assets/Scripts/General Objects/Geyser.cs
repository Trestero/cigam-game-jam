using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    private float waitTime = 1.5f;
    private float wTimer = 0.0f;
    private float geyserTime = 2.0f;
    private float gTimer = 0.0f;
    private bool isActivate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActivate)
        {
            wTimer += Time.deltaTime;
        }
        else
        {
            gTimer += Time.deltaTime;
            transform.localScale += new Vector3(0.0f, 0.1f, 0.0f);
        }
        if(wTimer > waitTime)
        {
            wTimer -= waitTime;
            isActivate = true;
        }
        if (gTimer > geyserTime)
        {
            gTimer -= geyserTime;
            isActivate = false;
        }
    }
}
