﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Transform[] t_bounds;
    Vector3[] bounds;
    [SerializeField] float speed;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        //get the positions of the starting and ending points (from prefab's children)
        t_bounds = gameObject.GetComponentsInChildren<Transform>();
        bounds = new Vector3[2];
        bounds[0] = t_bounds[0].position;
        bounds[1] = t_bounds[1].position;

        //snap platform to the center point of it's movement
        Vector3 startingPos = (bounds[0] + bounds[1]) / 2;
        transform.position = startingPos;

        direction = Vector3.Normalize(bounds[1] - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        Debug.Log(transform.position);
        //past bounds
        if(transform.position.x <= bounds[0].x || transform.position.x >= bounds[1].x)
        {
            direction *= -1;
        }

        transform.position += direction * speed;
    }
}
