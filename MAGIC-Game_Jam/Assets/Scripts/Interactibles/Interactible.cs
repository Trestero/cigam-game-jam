using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    protected bool isActive = false;
    public bool GetIsActive() { return isActive; }
    public void SetIsActive(bool value) { isActive = value; }

    protected bool playerInTrigger;

    BoxCollider trigger;
    GameObject player;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        trigger = gameObject.GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            isActive = !isActive;
        }
        if(playerInTrigger && isActive)
        {
            UseInteractible();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInTrigger = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("set is active");
                isActive = !isActive;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
