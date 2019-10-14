using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    protected bool isActive = false;
    public bool GetIsActive() { return isActive; }
    public void SetIsActive(bool value) { isActive = value; }

    protected bool playerInTrigger;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            isActive = !isActive;
        }
        if(isActive)
        {
            UseInteractible();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInTrigger = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInTrigger = false;
        }
    }

    protected abstract void UseInteractible();
}
