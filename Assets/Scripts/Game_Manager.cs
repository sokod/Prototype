using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public GameObject deathFloor;
    public GameObject player;
    public float speed;
    private static float factor;
    private static bool slowMotionEnabled=false;
    private float distanceToActor;

    public static void StartSlowMotion(float slowDownFactor)
    {
        factor += slowDownFactor;
        Time.timeScale = 1f/slowDownFactor;
        Time.fixedDeltaTime /= slowDownFactor;
        slowMotionEnabled = true;
    }
    public static void StopSlowMotion()
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
    public static float GetAngle(Vector2 directionNorm)
    {
        return Mathf.Atan2(directionNorm.y, directionNorm.x) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// получить угол поворота
    /// </summary>
    /// <param name="start"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 start, Vector3 target)
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
    public static void SetAngle(Vector2 directionNorm,Transform transform)
    {
        transform.rotation = Quaternion.AngleAxis(GetAngle(directionNorm) - 90, Vector3.forward);
    }


    private void Update()
    {
        distanceToActor = (deathFloor.transform.position - player.transform.position).sqrMagnitude; //дистанция лавы к игроку || експерементально
    }

    private void FixedUpdate()
    {
        float localSpeed;
        if (distanceToActor > 50)
            localSpeed = 2;
        else localSpeed = speed;
        deathFloor.transform.position += Vector3.up * Time.fixedDeltaTime* localSpeed; // плавно перемещаем тригер конца вверх со скоростью speed || експерементально
    }

}
