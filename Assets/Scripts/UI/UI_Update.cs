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
    public float penalty;
    //форс апдейт очков
    public bool forceUpdate;
    /// <summary>
    /// экран game over
    /// </summary>
    public GameObject DeadScreen;
    // текст с очками
    public Text HighestScore;

    private float highScore;
    private void Awake()
    {
        if (Instance == null)
        { // Экземпляр менеджера был найден
            Instance = this; // Задаем ссылку на экземпляр объекта
        }
        else if (Instance != null)
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
            Debug.LogWarning("More than one instances");
        }

        text = GetComponentInChildren<Text>();
    }
    /// <summary>
    /// вывод очков на экран
    /// </summary>
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //рестарт
    }
    /// <summary>
    /// вернуться на первую сцену
    /// </summary>
    public void ToMenu()
    {
        Game_Manager.Instance.StopSlowMotion();
        SceneManager.LoadScene(0);
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
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
