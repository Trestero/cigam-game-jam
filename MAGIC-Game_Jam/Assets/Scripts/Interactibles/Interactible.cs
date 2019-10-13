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

        Debug.Log("Is Active: " + isActive);
        Debug.Log("In trigger: " + playerInTrigger);
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
        Debug.Log("oh ok bye");
        if(other.gameObject.tag == "Player")
        {
            playerInTrigger = false;
        }
    }

    protected abstract void UseInteractible();



    void OnGUI()
    {
        if (playerInTrigger)
        {
            GUI.Box(new Rect(transform.position.x - 50, transform.position.y - 50, 100, 100), "Press E to use Interactable");
        }
    }
}
