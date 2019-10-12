using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector3[] bounds;
    [SerializeField] float speed;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        //get positions of the bounds
        bounds = new Vector3[2];
        bounds[0] = gameObject.GetComponentInParent<Transform>().position;
        bounds[1] = transform.parent.GetChild(0).transform.position;

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
        //past bounds
        if(!(transform.position.x > bounds[0].x && transform.position.x < bounds[1].x))
        {
            Debug.Log("lower: " + bounds[0]);
            Debug.Log("upper: " + bounds[1]);
            Debug.Log("position: " + transform.position);
            direction *= -1;
        }

        transform.position += direction * speed;
    }
}
