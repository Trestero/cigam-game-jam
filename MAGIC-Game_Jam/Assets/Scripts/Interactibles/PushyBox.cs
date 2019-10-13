using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushyBox : Interactible
{
    GameObject player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isActive)
        {
            transform.SetParent(null);
        }
    }

    protected override void UseInteractible()
    {
        if (isActive)
        {
            //transform.SetParent(player.transform);
            transform.SetParent(player.transform);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {

    }
}
