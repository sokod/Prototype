using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_Update : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    private float highScore=0;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (text)
            ShowScore(text);
    }

    void ShowScore(Text text)
    {
            if (Game_Manager.Instance.gameScore > highScore)
            {
                highScore = Game_Manager.Instance.gameScore;
                text.text = string.Format($"Score: {highScore:f0}");
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
