using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_Update : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            text = GetComponentInChildren<Text>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (text)
            UpdateScore();
    }

    void UpdateScore()
    {
        float score = Game_Manager.Instance.gameScore * 2-Game_Manager.Instance.penalty;
            text.text = string.Format($"Score: {score:f0}");
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
