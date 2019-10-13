using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Material skyMat = null;
    [SerializeField] private Material UIMat = null;

    private Color earth1 = new Color(0.93f, 0.91f, 0.46f);
    private Color earth2 = new Color(0.22f, 0.83f, 1.0f);
    private Color hell1 = new Color(0.29f, 0.0f, 0.37f);
    private Color hell2 = new Color(0.0f, 0.0f, 0.0f);

    private float colorSpeed = 0.1f;

    private Text[] text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentsInChildren<Text>();

        //Set default values
        skyMat.SetColor("_Color1", earth1);
        skyMat.SetColor("_Color2", earth2);

        UIMat.SetColor("_ContourColor", Color.black);

        foreach (Text t in text)
        {
            t.color = Color.black;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        skyMat.SetColor("_Color1", Color.Lerp(earth1, hell1, Mathf.PingPong(Time.time * colorSpeed, 1)));
        skyMat.SetColor("_Color2", Color.Lerp(earth2, hell2, Mathf.PingPong(Time.time * colorSpeed, 1)));

        UIMat.SetColor("_ContourColor", Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time * colorSpeed, 1)));

        foreach (Text t in text)
        {
            t.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time * colorSpeed, 1));
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
