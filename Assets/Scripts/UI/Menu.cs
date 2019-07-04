using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject customize_panel;
    public GameObject player_panel;
    public GameObject jump_effect_panel;
    public GameObject collision_effect_panel;
    public GameObject arrow_panel;

    public void StartGame()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Quit()
    {
        Application.Quit();
    }

    /*
    public void CustomizeSwitch()
    {
        if (!customize_panel.activeInHierarchy)
        customize_panel.SetActive(true);
        else customize_panel.SetActive(false);

    }*/

        /// <summary>
        /// отображаем панель в зависимости от нажатой кнопки
        /// </summary>
    public void ShowPanel()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Player":
                player_panel.SetActive(true);
                break;
            case "Jump Effects":
                jump_effect_panel.SetActive(true);
                break;
            case "Collision Effects":
                collision_effect_panel.SetActive(true);
                break;
            case "Arrow":
                arrow_panel.SetActive(true);
                break;
            case "Customize":
                customize_panel.SetActive(true);
                break;
            case "BackToMenu":
                DisableAll();
                break;
            case "Back":
                EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
                break;
            default:
                break;

        }
    }
    /// <summary>
    /// скрыть все панели
    /// </summary>
    private void DisableAll()
    {
        customize_panel.SetActive(false);
        jump_effect_panel.SetActive(false);
        player_panel.SetActive(false);
        arrow_panel.SetActive(false);
        collision_effect_panel.SetActive(false);
    }
}
