﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector3[] bounds;
    [SerializeField] float speed;
    GameObject player;
    float timer = 0;
    bool lerpUp = true;

    // Start is called before the first frame update
    void Start()
    {
        //get positions of the bounds
        bounds = new Vector3[2];
        bounds[0] = transform.parent.GetChild(0).transform.position;
        bounds[1] = transform.parent.GetChild(1).transform.position;

        //snap platform to the center point of it's movement
        Vector3 startingPos = bounds[0];
        transform.position = startingPos;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        if (lerpUp)
        {
            timer += Time.deltaTime * speed;
        }
        else
        {
            timer -= Time.deltaTime * speed;
        }
        transform.position = Vector3.Lerp(bounds[0], bounds[1], timer);

        if(timer >= 1 || timer <= 0)
        {
            lerpUp = !lerpUp;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        player.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        player.transform.parent = null;
    }
}
