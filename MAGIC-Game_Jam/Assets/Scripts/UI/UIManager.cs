using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    private GameObject pauseMenu = null;
    private Button pauseButton = null;

    // Start is called before the first frame update
    void Start()
    {
        pauseButton = GameObject.Find("Pause Button").GetComponent<Button>();

        pauseMenu = GameObject.Find("Pause Menu");
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
            this.enabled = false;
            pauseMenu.SetActive(true);
        }
        else if(Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
            this.enabled = true;
            pauseMenu.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}