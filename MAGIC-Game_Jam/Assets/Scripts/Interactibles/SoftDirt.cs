using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftDirt : Interactible
{
    SoftDirt other;
    Vector3 scale;
    public float GetZScale() { return scale.z; }
    public void SetZScale(float value) { scale = new Vector3(scale.x, scale.y, value); }

    [SerializeField] float pushMultiplier = 1;
    [SerializeField] float pushThreshold = 0.01f;
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
        if(speed > pushThreshold && scale.z > 0)
        {
            scale.z -= speed * pushMultiplier;
            other.SetZScale(other.GetZScale() + speed * pushMultiplier);
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
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player on soft dirt");
            speed = -col.gameObject.GetComponent<PlayerMovement>().GetLandingVelocity();
            //Debug.Log(speed);
        }
    }
}
