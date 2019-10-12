using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactible
{
    [SerializeField] GameObject platform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            platform.GetComponent<MovingPlatform>().Move();
        }
    }
}
