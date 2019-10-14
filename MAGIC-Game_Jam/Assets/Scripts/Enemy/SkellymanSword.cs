using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellymanSword : MonoBehaviour
{
    private void Start()
    {
        swordCollider = GetComponent<Collider>();
    }

    private Collider swordCollider;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GameOver();
        }
    }

    public void Toggle(bool val)
    {
        swordCollider.enabled = val;
    }
}
