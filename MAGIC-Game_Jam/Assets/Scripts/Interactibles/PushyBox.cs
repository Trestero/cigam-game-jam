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
    }

    protected override void UseInteractible()
    {
        if (transform.parent == null)
        {
            //transform.SetParent(player.transform);
            transform.parent = player.transform;
        }
        else
        {
            Debug.Log("why not work?");
            //transform.SetParent(null);
            transform.parent = null;
        }
    }
}
