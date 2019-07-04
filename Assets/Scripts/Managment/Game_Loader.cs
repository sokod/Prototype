using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_Loader : MonoBehaviour
{
    private GameObject player_body;
    public GameObject jumpParticleEffect; // need solution
    public GameObject collisionEffect; // need solution
    private GameObject arrowHead;

    public static Game_Loader Instance;

    public List<GameObject> bodyPrefabs = new List<GameObject>();
    public List<GameObject> jumpEffectsPrefabs = new List<GameObject>();
    public List<GameObject> collisionEffectPrefabs = new List<GameObject>();
    public List<GameObject> arrowHeadPrefabs = new List<GameObject>();

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
        LoadBuild();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// изменяем выбранный объект на новый
    /// </summary>
    /// <param name="selectedObject"></param>
    public void ChangeSelectedObject(GameObject selectedObject)
    {
        switch (GetId(selectedObject))
        {
            case "PlayerBody":
                player_body = selectedObject;
                break;
            case "JumpParticleEffect":
                jumpParticleEffect = selectedObject;
                break;
            case "CollisionEffect":
                collisionEffect = selectedObject;
                break;
            case "ArrowHead":
                arrowHead = selectedObject;
                break;
            default:
                Debug.LogError("Wrong object");
                break;

        }
    }
    /// <summary>
    /// получить выбранный объект
    /// </summary>
    /// <param name="selectedObject"></param>
    /// <returns></returns>
    public GameObject GetPrefabInLoader(GameObject selectedObject)
    {
        switch (GetId(selectedObject))
        {
            case "PlayerBody":
                return player_body;
            case "JumpParticleEffect":
                return jumpParticleEffect;
            case "CollisionEffect":
                return collisionEffect;
            case "ArrowHead":
                return arrowHead;
            default:
                Debug.LogError("Wrong object");
                break;

        }
        Debug.LogWarning(GetId(selectedObject));
        return null; 
    }
    /// <summary>
    /// Обрезает вторую часть id (PlayerBody_custom и возвращает PlayerBody). 
    /// </summary>
    /// <param name="selectedObject"></param>
    /// <returns></returns>
    private string GetId(GameObject selectedObject)
    {
        string[] name = selectedObject.name.Split(new char[] { ' ', '_' });
        string id;
        if (name.Length > 0)
            id = name[0];
        else id = selectedObject.name;
        return id;
    }
    /// <summary>
    /// Устанавливаем текущие элементы игрока на загруженные из PlayerPrefs. 
    /// </summary>
    void LoadBuild()
    {
        string name = PlayerPrefs.GetString("PlayerBody","PlayerBody");
        foreach (GameObject body in bodyPrefabs)
        {
            if (body.name == name)
                player_body = body;
        }
        name = PlayerPrefs.GetString("ArrowHead","ArrowHead");
        foreach(GameObject arrow in arrowHeadPrefabs)
        {
            if (arrow.name == name)
                arrowHead = arrow;
        }
    }

    public void SaveCurrentBuild()
    {
        if (PlayerPrefs.GetString("PlayerBody") != player_body.name)
            PlayerPrefs.SetString("PlayerBody", player_body.name);

        if (PlayerPrefs.GetString("ArrowHead") != arrowHead.name)
            PlayerPrefs.SetString("ArrowHead", arrowHead.name);

        Debug.Log("Updating save build");
    }


    public void SetScene()
    {
        GameObject parent = GameObject.FindGameObjectWithTag("Player");
        if (!parent) return;
         Instantiate(jumpParticleEffect, parent.transform.position, Quaternion.identity, parent.transform);
         Instantiate(collisionEffect, parent.transform.position, Quaternion.identity, parent.transform);
         Instantiate(arrowHead, parent.transform.position, Quaternion.identity, parent.transform);
         Instantiate(player_body, parent.transform.position, Quaternion.identity, parent.transform); 
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

}
