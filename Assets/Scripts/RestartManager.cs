using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartManager : MonoBehaviour
{

    public GameObject Editor;

    public GameObject WinCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();

	}

    public void Restart()
    {
        Editor.SetActive(true);
        Editor.GetComponentInChildren<MouseManager>().CreateRoot();
    }

    public void Win()
    {
        WinCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
