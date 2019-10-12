using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> interactables;
    private bool interactableNear = false;
    private GameObject closest;

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
            if (player.transform.position.x > interactable.transform.position.x - 3 &&
                player.transform.position.x < interactable.transform.position.x + 3 &&
                player.transform.position.z > interactable.transform.position.z - 3 &&
                player.transform.position.z < interactable.transform.position.z + 3)
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


                if (Input.GetKey(KeyCode.E))
                {
                    interactable.transform.position += new Vector3(1, 0, 0);
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
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Press E to use Interactable");
        }
    }
}
