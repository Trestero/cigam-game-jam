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
            gameObject.GetComponent<Rigidbody>().isKinematic = false;

            transform.SetParent(null);
        }
    }

    protected override void UseInteractible()
    {

            //transform.SetParent(player.transform);
            transform.SetParent(player.transform);

            //I sincerely apologize to whichever cursed individual is forced to read this line of code
            //just know that writing it lost me enough cosmic kharma to guarantee an afterlife in the 7th circle of hell
            transform.position = new Vector3(transform.parent.position.x + transform.parent.gameObject.GetComponent<CapsuleCollider>().radius + gameObject.GetComponent<BoxCollider>().bounds.size.x / 2 + 0.1f, 
                transform.parent.position.y + transform.parent.gameObject.GetComponent<CapsuleCollider>().height / 2, 
                transform.parent.position.z);

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        
    }
}
