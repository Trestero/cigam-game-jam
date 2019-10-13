using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    private float timer = 0.0f;
    [SerializeField] private float actionTime = 2.0f;
    private int option = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (option == 1) //rising up
        {
            transform.localScale += new Vector3(0.0f, 0.0f, 2f);
        }
        else if (option >= 2) //going down
        {
            transform.localScale -= new Vector3(0.0f, 0.0f, 2f);
        }
        if (timer > actionTime)
        {
            if(option >= 2)
            {
                option = 0; //waiting
            }
            else
            {
                option++;
            }
            timer -= actionTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(option >= 1) //active
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver(); //haha you lose idiot
        }
    }
}
