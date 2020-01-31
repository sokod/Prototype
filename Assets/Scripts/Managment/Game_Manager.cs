using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private GameObject deathFloor;
    private GameObject player;
    public float floorSpeed; // regulate speed in Inspector;
    public float playerMoveScore { private set; get; }
    public static Game_Manager Instance;

    private float slowDownFactor;
    public bool slowMotionEnabled {get; private set;}
    private Vector3 deathFloorUpForceVector;

    public float startFloorSpeed { private set; get;}

    private void Awake()
    {
        if (Instance == null)
        { // Экземпляр менеджера был найден
            Instance = this; // Задаем ссылку на экземпляр объекта

            deathFloor = GameObject.FindGameObjectWithTag("Finish");
            Game_Loader.Instance.SetScene();
            player = GameObject.FindGameObjectsWithTag("Player")[1];
            startFloorSpeed = floorSpeed;
            slowMotionEnabled = false;
        }
        else if (Instance != null)
        { // Экземпляр объекта уже существует на сцене
            Destroy(gameObject); // Удаляем объект
            Debug.LogWarning("More than one instances");
            return;
        }
    }

    /// <summary>
    /// замедление времени
    /// </summary>
    /// <param name="slowDownFactor"> фактор замедления </param>
    public void StartSlowMotion(float slowDownFactor)
    {
        if (!slowMotionEnabled)
        {
            this.slowDownFactor = slowDownFactor;
            Time.timeScale = 1f / slowDownFactor;
            Time.fixedDeltaTime /= slowDownFactor;
            slowMotionEnabled = true;
            StartCoroutine(PenaltyForSlowMotion());
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
            Time.fixedDeltaTime *= slowDownFactor;
            slowDownFactor = 0f;
            slowMotionEnabled = false;
        }
    }

    IEnumerator PenaltyForSlowMotion()
    {
        int timer=0;
        int sizeDefault = UI_Update.Instance.text.fontSize;
        while (slowMotionEnabled && !UI_Update.Instance.IsPaused)
        {
            timer++;
            if (timer > 14)
            {
                UI_Update.Instance.penalty += 0.1f* (float)timer/10f;
                UI_Update.Instance.text.color = new Color(UI_Update.Instance.text.color.r, UI_Update.Instance.text.color.g-0.05f, UI_Update.Instance.text.color.b-0.05f);
                UI_Update.Instance.text.fontSize = Mathf.Clamp(UI_Update.Instance.text.fontSize+1,80,100);
                if (UI_Update.Instance.text.fontSize == 100) UI_Update.Instance.text.fontSize = 80;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
        UI_Update.Instance.text.color = Color.white;
        UI_Update.Instance.text.fontSize = sizeDefault;
        Debug.Log("Penalty initialized");
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
         //дистанция лавы к игроку || експерементально
        if (Time.frameCount % 5 == 0) //каждые 5 кадров проверяем позицию игрока и обновляем очки
        {
            playerMoveScore = player.transform.localPosition.y;
            UI_Update.Instance.ShowScore();
        }
        if (!UI_Update.Instance.IsPaused)
            deathFloorUpForceVector = DeathFloorPositionUpdate();
        else deathFloorUpForceVector = Vector3.zero;
    }


    private Vector3 DeathFloorPositionUpdate()
    {
        float distanceToActor = (deathFloor.transform.position - player.transform.position).sqrMagnitude;
        if (distanceToActor > 130 && distanceToActor<400)
        {
            float currentFloorSpeed = 3;
            startFloorSpeed = floorSpeed;
            return Vector3.up * Time.fixedDeltaTime * (currentFloorSpeed);
        }
        else if (distanceToActor > 400)
        {
            float currentFloorSpeed = 12;
            startFloorSpeed = floorSpeed;
            return Vector3.up * Time.fixedDeltaTime * (currentFloorSpeed);
        }

        else
        {
            startFloorSpeed += Time.fixedDeltaTime*0.04f;
            return Vector3.up* Time.fixedDeltaTime * (startFloorSpeed);
        }
    }

    private void LateUpdate()
    {
        deathFloor.transform.position += deathFloorUpForceVector;  // плавно перемещаем тригер конца вверх со скоростью floorSpeed || експерементально
    }

}
