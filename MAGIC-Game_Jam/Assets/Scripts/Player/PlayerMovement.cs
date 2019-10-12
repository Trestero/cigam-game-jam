using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] float speed; //how quickly the player changes their velocity
    [SerializeField] float maxXVelocity; //how fast the player can go
    [SerializeField] float jumpForce;

    RaycastHit raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //This raycast is using a sphere collider for max distance, will probably have to change when model comes in
        Physics.Raycast(transform.position, Vector3.down, out raycastHit, gameObject.GetComponent<SphereCollider>().radius + 0.1f);

        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddForce(-speed, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddForce(speed, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space) && raycastHit.collider != null)
        {
            rigidbody.AddForce(0.0f, jumpForce, 0.0f);
        }


        ClampVelocity();
    }

    void ClampVelocity()
    {
        if(rigidbody.velocity.x > maxXVelocity)
        {
            rigidbody.velocity = new Vector3(maxXVelocity, rigidbody.velocity.y, rigidbody.velocity.z);
        }
        else if(rigidbody.velocity.x < -maxXVelocity)
        {
            rigidbody.velocity = new Vector3(-maxXVelocity, rigidbody.velocity.y, rigidbody.velocity.z);
        }
    }
}
