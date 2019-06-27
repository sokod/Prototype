using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject start_btn;
    public GameObject customize_btn;
    public GameObject customize_field;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OnCustomize()
    {
        if (!customize_field.activeInHierarchy)
        customize_field.SetActive(true);
        else customize_field.SetActive(false);

    }
    private void SetActiveMainMenu(bool active)
    {
    }
}
