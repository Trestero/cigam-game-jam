using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> interactables;
    private bool interactableNear = false;
    private GameObject closest;

    float range = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int counter = 0;
        //checks if player is near interactable and gives them the option to use them
        foreach (GameObject interactable in interactables)
        {
            //player is within interactable range
            if (player.transform.position.x > interactable.transform.position.x - range &&
                player.transform.position.x < interactable.transform.position.x + range)
            {
                interactableNear = true;

                if (closest == null)
                {
                    closest = interactable;
                }
                else if (closest != null || closest != interactable) //checks for if there is a closer interactable
                {
                    if (Vector3.Distance(player.transform.position, interactable.transform.position) < Vector3.Distance(player.transform.position, closest.transform.position))
                    {
                        closest = interactable;
                    }
                }


                if (Input.GetKeyDown(KeyCode.E))
                {
                    //forgive me father, for I have sinned
                    interactable.GetComponent<Interactible>().SetIsActive(!interactable.GetComponent<Interactible>().GetIsActive());
                }
            }
            else
            {
                counter++;
            }
        }
        //if none of the interactables were near, don't bring up the option
        if (counter == interactables.Count)
        {
            interactableNear = false;
        }
    }

    void OnGUI()
    {
        if(interactableNear)
        {
            //GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Press E to use Interactable");
        }
    }
}
