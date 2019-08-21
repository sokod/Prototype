using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_Update : MonoBehaviour
{
    private Text text;
    public static UI_Update Instance;
    //пенальти за пропуск порталов
    private float Penalty;
    public float penalty
    {
        get { return Penalty; }
        set {
            if (score > 0.1f)
            {
                Penalty = value;
                forceUpdate = true;
            }
            }
    }

    //форс апдейт очков
    private bool forceUpdate;
    public bool IsPaused { get; private set; }


    /// <summary>
    /// экран game over
    /// </summary>
    public GameObject DeadScreen;
    // текст с очками
    public Text HighestScore;

    private float highScore;
    public float score { private set; get; }
    private void Awake()
    {
        if (Instance == null)
        { // Экземпляр менеджера был найден
            Instance = this; // Задаем ссылку на экземпляр объекта

            IsPaused = false;
            text = GetComponentInChildren<Text>();
        }
        else if (Instance != null)
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
            Debug.LogWarning("More than one instances");
            return;
        }
    }
    /// <summary>
    /// вывод очков на экран
    /// </summary>
    public void ShowScore()
    {
        score = Game_Manager.Instance.playerMoveScore * 2 - Penalty;

        if (highScore < score || (forceUpdate && !DeadScreen.activeInHierarchy))
        {
            highScore = score;
            forceUpdate = false;
            text.text = string.Format($"Score: {score:f0}");
        }
    }
    /// <summary>
    /// обновление лучшего счета в PlayerPrefs
    /// </summary>
    public void UpdateHighScore()
    {
        if (PlayerPrefs.GetFloat("HighScore") < highScore)
            PlayerPrefs.SetFloat("HighScore", highScore);
    }
    /// <summary>
    /// загрузить след. сцену
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// выключить приложение
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
    /// <summary>
    /// перезапустить сцену.
    /// </summary>
    public void Restart()
    {
        Game_Manager.Instance.StopSlowMotion();
        DeadScreen.SetActive(false);
        ShowPauseButton(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //рестарт
    }

    /// <summary>
    /// вернуться на первую сцену
    /// </summary>
    public void ToMenu()
    {
        Game_Manager.Instance.StopSlowMotion();
        GameObject transition = GameObject.FindWithTag("SceneTransition");
        transition.GetComponent<Transitions>().SetTransition(0);
    }
    /// <summary>
    /// запустить паузу
    /// </summary>
    public void Pause()
    {
        Game_Manager.Instance.StopSlowMotion();
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
            IsPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            IsPaused = true;
        }
    }
    public void ShowPauseButton(bool state)
    {
        GameObject pause_btn = gameObject.transform.Find("Pause").gameObject;
        if (pause_btn)
        {
            pause_btn.SetActive(state);
        }
        else Debug.LogWarning("No Pause Button Found");
    }

    public void ActivatePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}
