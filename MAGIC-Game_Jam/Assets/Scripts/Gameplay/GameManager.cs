using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
    [SerializeField] private Material playerHellMat = null;
    private GameObject deadPlayer = null;
    private Vector3 playerEarthPosition;
    private float timer = 0.0f;
    private float stealthTime = 2.0f;
    private bool stealth = false;
    private bool stealthCooldown = false;

    [SerializeField] private CameraSystem camRig = null;
    [SerializeField] private GameObject topLevel = null;
    [SerializeField] private GameObject bottomLevel = null;

    [SerializeField] private PostProcessProfile ppp = null;
    private Vignette vignette;
    private float vignetteMax = 0.5f;

    private Grain grain;
    private float grainMax = 1.0f;

    private Bloom bloom;
    private float bloomMax = 20.0f;

    private ColorGrading colorGrading;
    private float hueShiftValue = 0.0f;

    // information used for tracking player properly between alive and dead

    // Start is called before the first frame update
    void Start()
    {
        playerEarthPosition = playerEarth.transform.position;

        //Get and reset post processing values
        vignette = ppp.GetSetting<Vignette>();
        vignette.intensity.Override(0.0f);

        grain = ppp.GetSetting<Grain>();
        grain.intensity.Override(0.0f);

        bloom = ppp.GetSetting<Bloom>();
        bloom.intensity.Override(4.0f);

        colorGrading = ppp.GetSetting<ColorGrading>();
        colorGrading.hueShift.Override(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("HOW SCREWED WE ARE: " + howScrewedAreWe);

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

            //If player is currently alive, record current pos and set camera follow target to hell player
            if (gameState == GameState.ALIVE)
            {
                playerEarthPosition = playerEarth.transform.position;
                gameState = GameState.DEAD;

                playerEarth.GetComponent<PlayerMovement>().enabled = false;
                GoToHell();
                camRig.SetFollowTarget(playerHell.transform);
            }
            //If player is currently dead, set the earth player back to the last recorded pos, and set camera follow target to earth player
            else if (gameState == GameState.DEAD)
            {
                gameState = GameState.ALIVE;
                playerEarth.transform.position = playerEarthPosition;
                camRig.ScreenRatio = 0.5f;

                playerEarth.GetComponent<PlayerMovement>().enabled = true;

                camRig.SetFollowTarget(playerEarth.transform);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && stealth == false && stealthCooldown == false && timer == 0.0f && gameState == GameState.DEAD)
        {
            stealth = true;

            //Make player transparent
            Color playerColor = playerHellMat.GetColor("_ContourColor");
            playerColor.a = 0.2f;
            playerHellMat.SetColor("_ContourColor", playerColor);
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

                //Make player Opaque
                Color playerColor = playerHellMat.GetColor("_ContourColor");
                playerColor.a = 1.0f;
                playerHellMat.SetColor("_ContourColor", playerColor);
            }
            else //cooldown done
            {
                stealthCooldown = false;
            }
        }

        //Post Processing
        float lerpValue = (camRig.ScreenRatio - 0.5f) * 2.0f;
        vignette.intensity.Override(Mathf.Lerp(0.0f, vignetteMax, lerpValue));
        grain.intensity.Override(Mathf.Lerp(0.0f, grainMax, lerpValue));
        bloom.intensity.Override(Mathf.Lerp(4.0f, bloomMax, lerpValue));

        //Hue shift fun
        /*hueShiftValue += 01f;
        if (hueShiftValue >= 180.0f)
            hueShiftValue = -180.0f;
        colorGrading.hueShift.Override(hueShiftValue);*/
    }

    public bool IsInStealth()
    {
        return stealth;
    }

    public Transform GetPlayer()
    {
        if(gameState == GameState.ALIVE)
        {
            return playerEarth.transform;
        }
        else
        {
            return playerHell.transform;
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
        Debug.Log("ya ded");
    }

    // Spawns a player in the underworld
    void GoToHell()
    {
        Vector3 spawnPos = GetEquivalentPosition(topLevel.transform, bottomLevel.transform, playerEarth.transform);
        //deadPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        playerHell.transform.position = GetEquivalentPosition(topLevel.transform, bottomLevel.transform, playerEarth.transform);
    }


    // From: The point of reference currently used
    // To: The point of reference to get the equivalent for
    // Obj - The object whose position is being converted
    private Vector3 GetEquivalentPosition(Transform from, Transform to, Transform obj)
    {
        return to.position + (obj.position - from.position);
    }
}
