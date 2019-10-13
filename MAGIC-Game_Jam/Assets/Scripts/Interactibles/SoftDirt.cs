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
    bool shrinking = false;

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

        if(speed < -pushThreshold)
        {
            Debug.Log(speed);
            scale.z += speed * pushMultiplier;
            other.SetZScale(other.GetZScale() - speed * 1.5f);
            speed = 0;
        }

        if (scale.z != transform.localScale.z)
        {
            Debug.Log("changing scales");
            transform.localScale = scale;
        }
    }

    protected override void UseInteractible()
    {
        if (transform.localScale.z > 0)
        {
            shrinking = true;
        }
        else
        {
            shrinking = false;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(speed);
            speed = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity.y;
        }
    }
}
