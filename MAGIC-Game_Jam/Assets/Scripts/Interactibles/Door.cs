using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Movable
{
    // Start is called before the first frame update
    void Start()
    {
        //get positions of the bounds
        bounds = new Vector3[2];
        bounds[0] = transform.parent.GetChild(0).transform.position;
        bounds[1] = transform.parent.GetChild(1).transform.position;

        //snap platform to the center point of it's movement
        transform.position = bounds[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move()
    {
        timer += Time.deltaTime * speed;

        transform.position = Vector3.Lerp(bounds[0], bounds[1], timer);
        Debug.Log(bounds[0]);
        Debug.Log(bounds[1]);
    }
}
