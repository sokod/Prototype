using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public GameObject customize_panel;
    public GameObject player_panel;
    public GameObject jump_effect_panel;
    public GameObject collision_effect_panel;
    public GameObject arrow_panel;
    public GameObject gems_panel;
    private Text gems;

    public void Awake()
    {
        gems = gems_panel.GetComponentInChildren<Text>();
        gems.text = Game_Loader.Instance.gems.ToString();
    }

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
                gems_panel.SetActive(false);
                break;
            case "Jump Effects":
                jump_effect_panel.SetActive(true);
                gems_panel.SetActive(false);
                break;
            case "Collision Effects":
                collision_effect_panel.SetActive(true);
                gems_panel.SetActive(false);
                break;
            case "Arrow":
                arrow_panel.SetActive(true);
                gems_panel.SetActive(false);
                break;
            case "Customize":
                customize_panel.SetActive(true);
                break;
            case "BackToMenu":
                DisableAll();
                gems_panel.SetActive(true);
                break;
            case "Back":
                EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
                gems_panel.SetActive(true);
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
