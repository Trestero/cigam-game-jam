using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactible
{
    [SerializeField] GameObject platform;

    protected override void UseInteractible()
    {
        platform.GetComponent<MovingPlatform>().Move();
    }
}
