using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] float speed; //how quickly the player changes their velocity
    [SerializeField] float maxXVelocity; //how fast the player can go
    [SerializeField] float jumpForce;

    Collider[] charColliders; //Colliders used for ragdoll
    Animator anim;

    
    float landingVelocity;
    public float GetLandingVelocity() { return landingVelocity; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();

        charColliders = GetComponentsInChildren<Collider>();
        anim = GetComponentInChildren<Animator>();
        ToggleRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        Physics.Raycast(transform.position, Vector3.down, out raycastHit, 0.2f);
        Vector3 currentRotation = transform.rotation.eulerAngles;

        if (Input.GetKey(KeyCode.A))
        {
            //set rotation
            currentRotation.y = 180;
            transform.rotation = Quaternion.Euler(currentRotation);

            rigidbody.AddForce(-speed, 0.0f, 0.0f);

            if(anim)
                anim.SetBool("IsWalking", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //set rotation
            currentRotation.y = 0;
            transform.rotation = Quaternion.Euler(currentRotation);

            rigidbody.AddForce(speed, 0.0f, 0.0f);

            if (anim)
                anim.SetBool("IsWalking", true);
           
        }
        else
        {
            if (anim)
                anim.SetBool("IsWalking", false);
        }

        //if (Input.GetKeyDown(KeyCode.Space) && raycastHit.collider != null)
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(0.0f, jumpForce, 0.0f);
        }

        if(raycastHit.collider != null && rigidbody.velocity.y != 0)
        {
            landingVelocity = rigidbody.velocity.y;
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

    public void ToggleRagdoll()
    {
        //Get current ragdoll state
        bool isRagdoll = charColliders[1].enabled;

        //Set character colliders
        for (int i = 1; i < charColliders.Length; i++)
        {
            charColliders[i].enabled = !isRagdoll;
        }

        //Toggle animator
        anim.enabled = !anim.enabled;
    }
}
