using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Controller : MonoBehaviour
{
    #region Player control veriables
    private float maxStretch = 2f;
    private float jumpForce = 300f;
    private Touch touch; //касание
    private bool jumpReady; //натяжение достаточное для прыжка? Update
    private bool canJump;//флаг на прыжок для fixedUpdate
    private Vector2 pushForceDirection; //направление толчка
    private Vector2 standartScale; // стандартный размер игрока
    private Vector2 FirstTouch;
    private float maxStretchSqr, force; //макс натяжение в квадрате; сила толчка.
    private GameObject player; // главный объект игрока
    private Rigidbody2D player_rb; //компонент rigid body игрока
    #endregion Player control veriables

    #region Graphics
    private ParticleSystem jumpEffect;
    private LineRenderer arrowTail; //linerenderer стрелки
    private SpriteRenderer arrowHeadSprite;
    private GameObject arrowHead;
    private SpriteRenderer sprite;
    #endregion Graphics

    private Camera MainCamera;


    /// <summary>
    /// ищем нужные компоненты на сцене
    /// </summary>
    void Awake()
    {
        player = this.gameObject;
        player_rb = player.GetComponent<Rigidbody2D>();
        arrowTail = player.GetComponent<LineRenderer>();
        //
        arrowHead = GameObject.Find("ArrowHead(Clone)");
        jumpEffect = GameObject.FindGameObjectsWithTag("Particle System")[0].GetComponent<ParticleSystem>();

        arrowHeadSprite = arrowHead.GetComponent<SpriteRenderer>();
        MainCamera = Camera.main;

        MainCamera.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Follow = gameObject.transform;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxStretchSqr = maxStretch * maxStretch; //находим квадрат максимального отдягивания (так,вроде, быстрей).
        canJump = true;
        standartScale = player.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }
    void FixedUpdate()
    {
        //если касание завершено
        if (touch.phase == TouchPhase.Ended && jumpReady && (!UI_Update.Instance.IsPaused))
        {
            Jump(); // прыгаем
        }
    }
    private void LateUpdate()
    {
        if (player.transform.localScale.x < standartScale.x) // возвращаем объекту стандартную форму
            player.transform.localScale = new Vector2(player.transform.localScale.x + 0.2f*Time.fixedDeltaTime, player.transform.localScale.y);

        if (player.transform.localScale.y < standartScale.y)
            player.transform.localScale = new Vector2(player.transform.localScale.x, player.transform.localScale.y + 0.2f * Time.fixedDeltaTime);
    }

    /// <summary>
    /// реализация прыжка
    /// </summary>
    void Jump()
    {
        jumpReady = false; // jump is beign processed
        jumpEffect.transform.position = player.transform.position; // ставим частицы на игрока
        ResetPlayerMotion();
        player_rb.AddForce(pushForceDirection.normalized * (150 + force * jumpForce), ForceMode2D.Force); // толкаем в направлении нажатия с силой * jumpForce
        jumpEffect.Play(); // включаем выброс частиц
        // уменшаем героя
        player.transform.localScale *= Mathf.Clamp((1f - force / 4f), 0.6f, 0.85f);
        ParticleSystem.MainModule main = jumpEffect.main;
        main.startSpeedMultiplier = force * 3;
        sprite.color = new Color(sprite.color.r - 0.4f, sprite.color.g - 0.4f, sprite.color.b - 0.4f);

        canJump = false; // jump is done
    }

    private void ResetPlayerMotion()
    {
        player_rb.velocity = Vector2.zero; //обнуляем перемещение игрока
        player_rb.inertia = 0f;
        player_rb.angularVelocity = 0f;
    }

    /// <summary>
    ///  замедляем время, даем возможность менять натяжение
    /// </summary>
    /// <param name="touch">касание</param>
    private void Control()
    {
        if (!UI_Update.Instance.DeadScreen.activeInHierarchy) //только если не активно game over
        {
            
            if (Input.touchCount > 0)
            {
                //если регистрируем касание, то записываем информацию о нем в touch
                touch = Input.GetTouch(0);
                if (!UI_Update.Instance.IsPaused)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Game_Manager.Instance.StartSlowMotion(10);
                        FirstTouch = (Vector2)MainCamera.ScreenToWorldPoint(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        if (canJump)
                            Dragging(FirstTouch);
                    }
                    else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {
                        ClearArrow(); //убираем стрелку
                        Game_Manager.Instance.StopSlowMotion();//возвращаемся к нормальному времени
                    }
                }
            }
        }
        else ClearArrow();
    }

    /// <summary>
    /// расчет вектора и силы натяжения, рисование стрелки 
    /// </summary>
    private void Dragging(Vector2 firstTouchedWorldPoint)
    {

        Vector2 touchedWorldPoint = (Vector2)MainCamera.ScreenToWorldPoint(touch.position); //позиция нажатия относительно координат камеры
        pushForceDirection = touchedWorldPoint - firstTouchedWorldPoint; //расчитываем вектор натяжения
        Ray2D rayFromPlayer = new Ray2D(player.transform.position, pushForceDirection); // луч от центра игрока в направление натяжения
        Vector2 endLinePoint = rayFromPlayer.GetPoint(pushForceDirection.magnitude); // получаем вектор конца на точке натяжения

        Debug.DrawLine(firstTouchedWorldPoint, touchedWorldPoint, Color.blue); // вектор натяжения игрока изначальный
        Debug.DrawLine(player.transform.position, endLinePoint, Color.white); //вектор натяжения игрока скоректированный под центр объекта

        if (pushForceDirection.sqrMagnitude > maxStretchSqr) // если натяжение больше чем максимально допустимое
        {
            endLinePoint = rayFromPlayer.GetPoint(maxStretch); //Перезаписываем точку касания в точку по лучу на максимально допустимую дистанцию
            Debug.DrawLine(player.transform.position, endLinePoint, Color.red);
        }

        if (pushForceDirection.sqrMagnitude <= 0.3) // если натяжение слишком мало, совершить прыжок невозможно
        {
            jumpReady = false;
            ClearArrow(); //не рисуем стрелку
            Debug.DrawLine(player.transform.position, endLinePoint, Color.black);

        }
        else
        {
            jumpReady = true;
            force = (Mathf.Clamp(pushForceDirection.sqrMagnitude, 0f, maxStretchSqr)) / maxStretchSqr; // ограничиваем силу прыжка от 0 до 1;
            UpdateArrow(force,player.transform.position,endLinePoint); //обновляем стрелку
        }
    }

    /// <summary>
    /// очистка стрелки
    /// </summary>
    private void ClearArrow()
    {
        arrowTail.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
        arrowHeadSprite.color = new Color(1f, 1f, 1f, 0f);
    }

    /// <summary>
    /// обновление местоположения стрелки, обновление градиента цвета
    /// </summary>
    private void UpdateArrow(float force, Vector2 firstPoint, Vector2 endPoint)
    {
        arrowHead.transform.position = endPoint; // ставим стрелку в нажатую точку
        Gradient gradient = new Gradient();
        gradient.SetKeys( //обновляем цвет стрелки, чем больше сила - тем ярче цвет.
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(1f, 1f-force*0.6f, 1f-force*0.6f, 1f), 0.6f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 1f) }
                );

        arrowTail.colorGradient = gradient; //устанавливаем новый цвет
        arrowTail.SetPosition(0, firstPoint); //начальная точка от объекта игрока
        arrowTail.SetPosition(1, endPoint); //конечная точка к касанию
        arrowHeadSprite.color= new Color(1f, 1f - force*0.7f, 1f - force*0.7f, 0.8f); // обновляем цвет конца стрелки
        arrowHead.transform.localScale = new Vector2(Mathf.Clamp(0.15f + force*0.35f,0.15f,0.3f), Mathf.Clamp(0.15f + force*0.35f, 0.15f, 0.3f)); // обновляем размер стрелки
        // поварачиваем в сторону направления
        Game_Manager.Instance.SetAngle(pushForceDirection.normalized, arrowHead.transform);
        //Важно что бы частицы не были привязаны к трансформу игрока, иначе просчет поворота работать не будет.
        Game_Manager.Instance.SetAngle(pushForceDirection.normalized, jumpEffect.transform);
    }
    /// <summary>
    /// обнулить прыжки
    /// </summary>
    public void ResetJumps()
    {
        if (!canJump) sprite.color = new Color(sprite.color.r + 0.4f, sprite.color.g + 0.4f, sprite.color.b + 0.4f);
        canJump = true;
    }
}
