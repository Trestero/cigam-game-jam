using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public GameObject player;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //This raycast is using a sphere collider for max distance, will probably have to change when model comes in
        Physics.Raycast(transform.position, Vector3.down, out raycastHit, 0.1f);

        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddForce(-speed, 0.0f, 0.0f);
            GetComponentInChildren<Animator>().SetBool("IsWalking", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddForce(speed, 0.0f, 0.0f);
            GetComponentInChildren<Animator>().SetBool("IsWalking", true);

        }


        ClampVelocity();
    }

    void ClampVelocity()
    {
        if (rigidbody.velocity.x > maxXVelocity)
        {
            rigidbody.velocity = new Vector3(maxXVelocity, rigidbody.velocity.y, rigidbody.velocity.z);
        }
        else if (rigidbody.velocity.x < -maxXVelocity)
        {
            rigidbody.velocity = new Vector3(-maxXVelocity, rigidbody.velocity.y, rigidbody.velocity.z);
        }
    }
}
