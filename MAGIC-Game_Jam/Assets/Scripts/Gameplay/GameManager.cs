using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState { ALIVE, ALIVENT };
    private GameState gameState = GameState.ALIVE;

    [Header("Game Rules")]
    [SerializeField] private float secondsAllowedInHell = 60;
    private float timeSpentInHell = 0.0f;
    private float howScrewedAreWe = 0.0f;


    [Header("Gameplay Information")]
    [SerializeField] private GameObject player = null;
    [SerializeField] private CameraSystem camRig = null;

    // information used for tracking player properly between alive and dead

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.ALIVE:
                break;
            case GameState.ALIVENT:
                timeSpentInHell += Time.deltaTime;
                howScrewedAreWe = timeSpentInHell / secondsAllowedInHell;
                camRig.ScreenRatio = Mathf.Lerp(0.5f, 1.0f, howScrewedAreWe);
                break;
        }
    }

    void KillPlayer()
    {
        // TODO: Put earth-side player into dead mode, spawn player on the underside
        // Player.SetAliveState(false);
    }

    void GameOver()
    {

    }
}
