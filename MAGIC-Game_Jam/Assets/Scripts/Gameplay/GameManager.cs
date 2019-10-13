using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState { ALIVE, DEAD };
    private GameState gameState = GameState.ALIVE;

    [Header("Game Rules")]
    [SerializeField] private float secondsAllowedInHell = 20;
    private float timeSpentInHell = 0.0f;
    private float howScrewedAreWe = 0.0f;
    [SerializeField] private GameObject playerPrefab = null;


    [Header("Gameplay Information")]
    [SerializeField] private GameObject playerEarth = null;
    [SerializeField] private GameObject playerHell = null;
    private GameObject deadPlayer = null;
    private Vector3 playerEarthPosition;
    private float timer = 0.0f;
    private float stealthTime = 2.0f;
    private bool stealth = false;
    private bool stealthCooldown = false;

    [SerializeField] private CameraSystem camRig = null;
    [SerializeField] private GameObject topLevel = null;
    [SerializeField] private GameObject bottomLevel = null;

    // information used for tracking player properly between alive and dead

    // Start is called before the first frame update
    void Start()
    {
        playerEarthPosition = playerEarth.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.ALIVE:
                break;
            case GameState.DEAD:
                timeSpentInHell += Time.deltaTime;
                howScrewedAreWe = timeSpentInHell / secondsAllowedInHell;
                camRig.ScreenRatio = Mathf.Lerp(0.5f, 1.0f, howScrewedAreWe);
                break;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerEarth.GetComponent<PlayerMovement>().ToggleRagdoll();
            playerHell.SetActive(!playerHell.activeSelf);

            //If player is currently alive, record current pos and switch to dead
            if (gameState == GameState.ALIVE)
            {
                playerEarthPosition = playerEarth.transform.position;
                gameState = GameState.DEAD;

                playerEarth.GetComponent<PlayerMovement>().enabled = false;
            }
            //If player is currently dead, set the earth player back to the last recorded pos, and set state to alive
            else if (gameState == GameState.DEAD)
            {
                gameState = GameState.ALIVE;
                playerEarth.transform.position = playerEarthPosition;
                camRig.ScreenRatio = 0.5f;

                playerEarth.GetComponent<PlayerMovement>().enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && stealth == false && stealthCooldown == false && timer == 0.0f)
        {
            stealth = true;
        }
        if(stealth || stealthCooldown)
        {
            timer += Time.deltaTime;
        }
        if(timer >= stealthTime)
        {
            timer = 0.0f;
            if(stealth == true)
            {
                stealth = false;
                stealthCooldown = true;
            }
            else //cooldown done
            {
                stealthCooldown = false;
            }
        }
    }

    void KillPlayer()
    {
        // TODO: Put earth-side player into dead mode, spawn player on the underside
        // Player.SetAliveState(false);

        GoToHell();
    }

    public void GameOver()
    {

    }

    // Spawns a player in the underworld
    void GoToHell()
    {
        Vector3 spawnPos = GetEquivalentPosition(topLevel.transform, bottomLevel.transform, playerEarth.transform);
        deadPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

    }


    // From: The point of reference currently used
    // To: The point of reference to get the equivalent for
    // Obj - The object whose position is being converted
    private Vector3 GetEquivalentPosition(Transform from, Transform to, Transform obj)
    {
        return to.position + (obj.position - from.position);
    }
}
