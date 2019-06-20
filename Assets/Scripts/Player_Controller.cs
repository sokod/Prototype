using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public ParticleSystem jumpEffect;
    [Space(15)]
    public float maxStretch, jumpForce; // регулировка дистанции отдягивания, силы прыжка
    public int maxJumps; //максимальное кол-во прыжков

    private Vector2 touchedWorldPoint; //точка касания относительно камеры
    private Ray2D rayToTouchedPoint; // луч от центра объекта к точке касания
    private Touch touch; //касание
    private float maxStretchSqr,force; //макс натяжение в квадрате; сила толчка.
    private GameObject player; // главный объект игрока
    private bool jumpReady; //натяжение достаточное для прыжка?
    private int jumpCount;// параметр, отвечающий за возможность игрока совершать прыжок
    private Rigidbody2D player_rb; //компонент rigid body игрока
    private Vector2 pushForceDirection; //направление толчка
    private Vector2 standartScale; // стандартный размер игрока

    private LineRenderer arrowTail; //linerenderer стрелки
    private SpriteRenderer arrowHeadSprite;
    public GameObject arrowHead;

    /// <summary>
    /// ищем нужные компоненты на сцене
    /// </summary>
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player_rb = player.GetComponent<Rigidbody2D>();
        arrowTail = player.GetComponent<LineRenderer>();
        arrowHeadSprite = arrowHead.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxStretchSqr = maxStretch * maxStretch; //находим квадрат максимального отдягивания (так,вроде, быстрей).
        jumpCount = maxJumps;
        standartScale = player.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            //если регистрируем касание, то записываем информацию о нем в touch
            touch = Input.GetTouch(0);
            // инициируем перемещение 
            Control(touch);
        }
    }
    void FixedUpdate()
    {
        //если касание завершено
        if (touch.phase == TouchPhase.Ended && jumpReady)
        {
            Jump(); // прыгаем
        }
    }
    private void LateUpdate()
    {
        if (player.transform.localScale.x < standartScale.x)
            player.transform.localScale = new Vector2(player.transform.localScale.x + 0.8f*Time.fixedDeltaTime, player.transform.localScale.y+ 0.8f * Time.fixedDeltaTime);
    }

    /// <summary>
    /// реализация прыжка
    /// </summary>
    void Jump()
    {
        player_rb.velocity = Vector2.zero; //обнуляем перемещение игрока
        player_rb.inertia = 0f;
        player_rb.angularVelocity = 0f;
        player_rb.AddForce(pushForceDirection.normalized * (100 + force * jumpForce), ForceMode2D.Force); // толкаем в направлении нажатия с силой * jumpForce
        jumpReady = false;
        jumpCount--;
        jumpEffect.Play(); // включаем выброс частиц
        player.transform.localScale *= 0.7f;// уменшаем героя
        ParticleSystem.MainModule main = jumpEffect.main;
        main.startSpeedMultiplier = force*3;
    }

    /// <summary>
    ///  замедляем время, даем возможность менять натяжение
    /// </summary>
    /// <param name="touch">касание</param>
    private void Control(Touch touch)
    {
        //смотрим на фазы касания
        if (touch.phase == TouchPhase.Began && CanJump())
            Game_Manager.StartSlowMotion(10);

        switch (touch.phase)
        {
            case TouchPhase.Began: //при регистрации касания замедляем время
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (CanJump())
                    Dragging();
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                ClearArrow(); //убираем стрелку
                Game_Manager.StopSlowMotion();//возвращаемся к нормальному времени
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Проверка на возможность прыжка
    /// </summary>
    /// <returns></returns>
    public bool CanJump()
    {
        if (jumpCount > 0)
        {
            return true;
        }
        else return false;
    }

    /// <summary>
    /// расчет вектора и силы натяжения, рисование стрелки 
    /// </summary>
    private void Dragging()
    {
        touchedWorldPoint = (Vector2)Camera.main.ScreenToWorldPoint(touch.position); //позиция нажатия относительно координат камеры
        pushForceDirection = touchedWorldPoint - (Vector2)player.transform.position; //расчитываем вектор натяжения
        if (pushForceDirection.sqrMagnitude > maxStretchSqr) // если натяжение больше чем максимально допустимое
        {
            rayToTouchedPoint = new Ray2D(player.transform.position, pushForceDirection); //задаем луч от центра объекта игрока к вектору направления
            touchedWorldPoint = rayToTouchedPoint.GetPoint(maxStretch); //Перезаписываем точку касания в точку по лучу на максимально допустимую дистанцию
        }

        if (pushForceDirection.sqrMagnitude <= 0.3 || pushForceDirection.sqrMagnitude>=30) // если натяжение слишком мало или большое
        {
            jumpReady = false;
            ClearArrow(); //не рисуем стрелку
            Debug.DrawLine(touchedWorldPoint, player.transform.position, Color.black);
            //return false; //не даем возможность прыгнуть
        }
        else
        {
            jumpReady = true;
            Debug.DrawLine(touchedWorldPoint, player.transform.position, Color.red);
            force = (Mathf.Clamp(pushForceDirection.sqrMagnitude, 0f, maxStretchSqr)) / maxStretchSqr; // ограничиваем силу прыжка от 0 до 1;
            UpdateArrow(force); //обновляем стрелку
        }
        arrowHead.transform.position = touchedWorldPoint; // ставим стрелку в нажатую точку
        jumpEffect.transform.position = player.transform.position; // ставим частицы на игрока
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
    private void UpdateArrow(float force)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys( //обновляем цвет стрелки, чем больше сила - тем ярче цвет.
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(1f, 1f-force*0.6f, 1f-force*0.6f, 1f), 0.6f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0f, 0.3f), new GradientAlphaKey(1f, 1.0f) }
                );

        arrowTail.colorGradient = gradient; //устанавливаем новый цвет
        arrowTail.SetPosition(0, (Vector2)player.transform.position); //начальная точка от объекта игрока
        arrowTail.SetPosition(1, (Vector2)touchedWorldPoint); //конечная точка к касанию
        arrowHeadSprite.color= new Color(1f, 1f - force*0.7f, 1f - force*0.7f, 0.8f); // обновляем цвет конца стрелки
        arrowHead.transform.localScale = new Vector2(Mathf.Clamp(0.25f + force*0.5f,0.25f,0.5f), Mathf.Clamp(0.4f + force*0.5f, 0.25f, 0.5f)); // обновляем размер стрелки

        //какая-то магия.
        float angle = Mathf.Atan2(pushForceDirection.y, pushForceDirection.x) * Mathf.Rad2Deg; // фиг знает что
        arrowHead.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward); // поворот стрелки 
        //Важно что бы частицы не были привязаны к трансформу игрока, иначе просчет поворота работать не будет.
        jumpEffect.transform.localRotation = Quaternion.AngleAxis(angle+90, Vector3.forward); //поворот системы частиц.  

    }
    
    public void ResetJumps()
    {
        jumpCount = maxJumps;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Simple Wall")
            Debug.LogWarning("Respawn is Necessary");
    }
}
