using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelFunctions : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private CameraSystem camRig;
    // Start is called before the first frame update
    void Start()
    {
            camRig.HardSetScreenRatio(0);
    }
}