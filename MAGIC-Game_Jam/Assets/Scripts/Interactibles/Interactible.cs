using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    protected bool isActive = false;
    public bool GetIsActive() { return isActive; }
    public void SetIsActive(bool value) { isActive = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(isActive)
        {
            UseInteractible();
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isActive = !isActive;
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        //isActive = false;
    }

    protected abstract void UseInteractible();
}
