using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Game_Loader : MonoBehaviour
{
    public GameObject player_body;
    public GameObject jumpParticleEffect;
    public GameObject collisionEffect;
    public GameObject arrowHead;
    public static Game_Loader Instance = null;

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

    void Check()
    {
        //добавить запись у файл выбраных вариантов у PlayerPref название объекта
        //после сравнивать с выбраным на данный момент и менять.
        //потом найти объект по названию и установить вместо текущего.
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

}
