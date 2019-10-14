using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftDirt : Interactible
{
    SoftDirt other;
    bool hasPhysicsObject = false;
    public bool GetHasPhyiscsObject() { return hasPhysicsObject; }

    Vector3 scale;
    public float GetZScale() { return scale.z; }
    public void SetZScale(float value) { scale = new Vector3(scale.x, scale.y, value); }

    [SerializeField] float pushMultiplier = 1;
    [SerializeField] float pushThreshold = 0.01f;
    [SerializeField] float physicsPush = 1.0f;
    float speed;

    // Start is called before the first frame update
    protected override void Start()
    {
        scale = transform.localScale;
        foreach (SoftDirt softDirt in transform.parent.GetComponentsInChildren<SoftDirt>())
        {
            if(softDirt != this)
            {
                other = softDirt;
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(speed > pushThreshold && transform.localScale.z > 0.011)
        {
            scale.z -= speed * pushMultiplier;
            other.SetZScale(other.GetZScale() + speed * pushMultiplier);
        }

        if(scale.z < 0.11f)
        {
            scale.z = 0.11f;
        }

        if (scale.z != transform.localScale.z)
        {
            transform.localScale = scale;
        }
        speed = 0;
    }

    protected override void UseInteractible()
    {

    }

    protected void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Physics")
        {
            speed = physicsPush;
            hasPhysicsObject = true;
        }
        else if (col.gameObject.tag == "Player" && !other.GetHasPhyiscsObject())
        {
            speed = -col.gameObject.GetComponent<PlayerMovement>().GetLandingVelocity();
        }
    }

    protected void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag == "Physics")
        {
            hasPhysicsObject = false;
        }
    }
}
