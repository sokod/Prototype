using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct SaveParticlesInUI
{
    public Sprite particlePicture;
    public GameObject particleEffect;
}

[System.Serializable]
public struct Blocks
{
    public string name;
    public GameObject block;
}

public class Game_Loader : MonoBehaviour
{
    private GameObject player_body;
    private GameObject jumpEffect; 
    private GameObject collisionEffect; 
    private GameObject arrowHead;
    public int gems { get; private set; }
    public static Game_Loader Instance;
    public List<GameObject> bodyPrefabs = new List<GameObject>();
    public List<GameObject> arrowHeadPrefabs = new List<GameObject>();
    public List<GameObject> jumpEffectPrefabs = new List<GameObject>();
    public List<GameObject> collisionEffectPrefabs = new List<GameObject>();
    //public SaveParticlesInUI[] jumpEffectsPrefabs;
   // public SaveParticlesInUI[] collisionEffectPrefabs;
    public Blocks[] blocks;
    private void Awake()
    {
        if (Instance == null)
        { // Экземпляр менеджера был найден
            Instance = this; // Задаем ссылку на экземпляр объекта
        }
        else if (Instance != null)
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
            Debug.LogWarning("Tried to create another instance");
            return;
        }
        LoadBuild();
        gems = 1000;
        //gems = PlayerPrefs.GetInt("Gems", 0);
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
            case "JumpEffect":
                jumpEffect = selectedObject;
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
            case "JumpEffect":
                return jumpEffect;
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
        name = PlayerPrefs.GetString("JumpEffect", "JumpEffect");
        foreach(GameObject jump in jumpEffectPrefabs)
        {
            if (jump.name == name)
                jumpEffect = jump;
        }
        name = PlayerPrefs.GetString("CollisionEffect", "CollisionEffect");
        foreach (GameObject CollisionEffect in collisionEffectPrefabs)
        {
            if (CollisionEffect.name == name)
                collisionEffect = CollisionEffect;
        }
        /*
        name = PlayerPrefs.GetString("JumpEffect", "JumpEffect");
        for (int i = 0; i < jumpEffectsPrefabs.Length; i++){
            if (jumpEffectsPrefabs[i].particleEffect.name == name)
                jumpEffect = jumpEffectsPrefabs[i].particleEffect;
        }

        name = PlayerPrefs.GetString("CollisionEffect", "CollisionEffect");
        for (int i = 0; i < collisionEffectPrefabs.Length; i++){
            if (collisionEffectPrefabs[i].particleEffect.name == name)
                collisionEffect = collisionEffectPrefabs[i].particleEffect;
        }
        */
    }

    public void SaveCurrentBuild()
    {
        if (PlayerPrefs.GetString("PlayerBody") != player_body.name)
            PlayerPrefs.SetString("PlayerBody", player_body.name);

        if (PlayerPrefs.GetString("ArrowHead") != arrowHead.name)
            PlayerPrefs.SetString("ArrowHead", arrowHead.name);

        if (PlayerPrefs.GetString("JumpEffect") != jumpEffect.name)
            PlayerPrefs.SetString("JumpEffect", jumpEffect.name);

        if (PlayerPrefs.GetString("CollisionEffect") != collisionEffect.name)
            PlayerPrefs.SetString("CollisionEffect", collisionEffect.name);

        Debug.Log("Updating save build");
    }

    public void SaveCurrentGems()
    {
        PlayerPrefs.SetInt("Gems", gems);
    }

    public bool UpdateGems(int value)
    {
        if (gems + value < 0)
            return false;
        else gems += value;
        SaveCurrentGems();
        Debug.Log("Updating Gems value " + gems +"  " + value);
        return true;
    }
    public void SetScene()
    {
        GameObject parent = GameObject.FindGameObjectWithTag("Player");
        if (!parent) return;
        else
        {
            Instantiate(jumpEffect, parent.transform.position, Quaternion.identity, parent.transform);
            Instantiate(collisionEffect, parent.transform.position, Quaternion.identity, parent.transform);
            Instantiate(arrowHead, parent.transform.position, Quaternion.identity, parent.transform);
            Instantiate(player_body, parent.transform.position, Quaternion.identity, parent.transform);
            //GameObject player_holder = new GameObject("Player_holder");
            //player_holder.transform.parent = parent.transform;
            //player_holder.transform.localPosition = Vector3.zero;
            //player_holder.transform.localScale = Vector3.one;
            //Instantiate(player_body, parent.transform.position, Quaternion.identity, player_holder.transform);
        }
    }
    public void Delete()
    {
        Destroy(gameObject);
    }
}
