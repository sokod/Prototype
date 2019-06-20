using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public GameObject deathFloor;
    public float speed;
    private static float factor;
    private static bool slowMotionEnabled=false;

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
    private void LateUpdate()
    {
        deathFloor.transform.position += Vector3.up * Time.fixedDeltaTime* speed; // плавно перемещаем тригер конца вверх со скоростью speed
    }
}
