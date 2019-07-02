﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private GameObject deathFloor;
    public GameObject player;
    public float floorSpeed;
    public float gameScore;
    public static Game_Manager Instance;

    private float factor;
    private bool slowMotionEnabled=false;
    private float distanceToActor;

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
        deathFloor = GameObject.FindGameObjectWithTag("Finish");
        //
        Game_Loader.Instance.SetScene();
        player = GameObject.FindGameObjectsWithTag("Player")[1];
    }

    /// <summary>
    /// замедление времени
    /// </summary>
    /// <param name="slowDownFactor"> фактор замедления </param>
    public void StartSlowMotion(float slowDownFactor)
    {
        if (!slowMotionEnabled)
        {
            factor = slowDownFactor;
            Time.timeScale = 1f / slowDownFactor;
            Time.fixedDeltaTime /= slowDownFactor;
            slowMotionEnabled = true;
        }
    }
    /// <summary>
    /// сбросить замедление
    /// </summary>
    public void StopSlowMotion()
    {
        if (slowMotionEnabled)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime *= factor;
            factor = 0f;
            slowMotionEnabled = false;
        }
    }

    /// <summary>
    /// получить угол поворота
    /// </summary>
    /// <param name="directionNorm"></param>
    /// <returns></returns>
    public float GetAngle(Vector2 directionNorm)
    {
        return Mathf.Atan2(directionNorm.y, directionNorm.x) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// получить угол поворота
    /// </summary>
    /// <param name="start"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public float GetAngle(Vector3 start, Vector3 target)
    {
        Vector3 direction = start - target;
        direction.Normalize();
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// Поворот объекта в сторону входящего направления
    /// </summary>
    /// <param name="directionNorm"></param>
    /// <param name="transform"></param>
    public void SetAngle(Vector2 directionNorm,Transform transform)
    {
        transform.rotation = Quaternion.AngleAxis(GetAngle(directionNorm) - 90, Vector3.forward);
    }


    private void Update()
    {
        distanceToActor = (deathFloor.transform.position - player.transform.position).sqrMagnitude; //дистанция лавы к игроку || експерементально
        if (Time.frameCount % 5 == 0) //каждые 5 кадров проверяем позицию игрока и обновляем очки
        {
            gameScore = player.transform.localPosition.y;
            UI_Update.Instance.ShowScore();
        }
    }

    private void FixedUpdate()
    {
        float localSpeed;
        if (distanceToActor > 300)
            localSpeed = 10;
        else localSpeed = floorSpeed;
        deathFloor.transform.position += Vector3.up * Time.fixedDeltaTime* (localSpeed + Time.timeSinceLevelLoad/20f); // плавно перемещаем тригер конца вверх со скоростью floorSpeed || експерементально
    }
}