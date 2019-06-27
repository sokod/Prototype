using UnityEngine;

public class Game_Loader : MonoBehaviour
{
    public GameObject player_body;
    public GameObject jumpParticleEffect;
    public GameObject collisionEffect;
    public GameObject arrowHead;

    public static Game_Loader Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerBody(GameObject player)
    {
        player_body = player;
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
