using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameCompleteMenu;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameOver(){
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
    }

    public void GameComplete(){
        Time.timeScale = 0;
        gameCompleteMenu.SetActive(true);
    }

    public void Home()
    {
        SceneManager.LoadScene("Mainmenu");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
