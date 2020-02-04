using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    bool isPaused;

    public GameObject paused;

    private void Start()
    {
        isPaused = false;
    }
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ChangePauseStatus();
        }
        if (Input.GetButtonDown("Menu"))
        {
            
            if (isPaused)
            {
                SceneManager.LoadScene(1);
            }
            ChangePauseStatus();
        }
    }

    public void ChangePauseStatus()
    {
        isPaused = !isPaused;
        UpdateGamePause();
    }

    void UpdateGamePause()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            GameManager.isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            GameManager.isPaused = false;
        }
        paused.SetActive(isPaused);
        
    }
}
