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

    private float highScore;
    private void Awake()
    {
        Instance = this;
            text = GetComponentInChildren<Text>();
    }

    public void UpdateScore()
    {
        float score = Game_Manager.Instance.gameScore * 2 - penalty;
        if (highScore < score || forceUpdate)
        {
            highScore = score;
            forceUpdate = false;
            text.text = string.Format($"Score: {score:f0}");
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
