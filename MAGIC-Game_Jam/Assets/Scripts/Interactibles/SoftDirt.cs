using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftDirt : Interactible
{
    SoftDirt other;
    Vector3 scale;
    public float GetZScale() { return scale.z; }
    public void SetZScale(float value) { scale = new Vector3(scale.x, scale.y, value); }

    [SerializeField] float speed;
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

        if (shrinking && scale.z > 0)
        {
            scale.z -= speed;
            other.SetZScale(other.GetZScale() + speed);

            if(scale.z < 0)
            {
                scale.z = 0;
            }
        }
        else
        {
            shrinking = false;
            isActive = false;
        }

        if (scale.z != transform.lossyScale.y)
        {
            Debug.Log("chaning scales");
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
}
