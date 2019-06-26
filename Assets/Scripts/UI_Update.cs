using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_Update : MonoBehaviour
{
    private Text text;
    public static UI_Update Instance;
    public float penalty;
    public bool forceUpdate;
    public GameObject DeadScreen;
    public Text HighestScore;

    private float highScore;
    private void Awake()
    {
        Instance = this;
            text = GetComponentInChildren<Text>();
    }

    public void ShowScore()
    {
        float score = Game_Manager.Instance.gameScore * 2 - penalty;
        if (highScore < score || (forceUpdate && !DeadScreen.activeInHierarchy))
        {
            highScore = score;
            forceUpdate = false;
            text.text = string.Format($"Score: {score:f0}");
        }
    }
    public void UpdateHighScore()
    {
        if (PlayerPrefs.GetFloat("HighScore") < highScore)
            PlayerPrefs.SetFloat("HighScore", highScore);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Game_Manager.Instance.StopSlowMotion();
        DeadScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //рестарт
    }

    public void ToMenu()
    {
        Game_Manager.Instance.StopSlowMotion();
        SceneManager.LoadScene(0);
    }
    public void Pause()
    {
        Game_Manager.Instance.StopSlowMotion();
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
