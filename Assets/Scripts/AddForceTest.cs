using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceTest : MonoBehaviour
{

    private Vector3 touchedWorldPoint; //точка касания относительно камеры
    private Ray2D rayToTouchedPoint; // луч от центра объекта к точке касания
    [Space(15)]
    public float maxStretch = 3.0f; // регулировка дистанции отдягивания 

    private Touch touch; //касание
    private float maxStretchSqr; //отдягивание в квадрате
    private GameObject player; // главный объект игрока
    private bool CanJump; // параметр, отвечающий за возможность игрока совершать прыжок
    private Rigidbody2D player_rb; //компонент rigid body игрока
    private LineRenderer arrow; //linerenderer стрелки
    private Vector2 pushForceDirection; //направление толчка

    /// <summary>
    /// ищем нужные компоненты на сцене
    /// </summary>
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player_rb = player.GetComponent<Rigidbody2D>();
        arrow = player.GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxStretchSqr = maxStretch * maxStretch; //находим квадрат максимального отдягивания (так,вроде, быстрей).
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
        if (touch.phase == TouchPhase.Ended)
        {
            // и если у игрока есть возможность совершить прыжок
            if (CanJump)
            {
                player_rb.velocity = Vector2.zero; //обнуляем перемещение игрока
                player_rb.AddForce(pushForceDirection.normalized * (Mathf.Clamp(pushForceDirection.sqrMagnitude,1f,maxStretchSqr)*100),ForceMode2D.Force);
                //присваиваем объекту силу для прыжка в направлении pushForceDirection. Силу расчитываем с параметра квадрат натяжения (от 1 до макс натяжения)*100
                CanJump = false; //убираем возможность прыжка
                ClearLine(); //убираем стрелку
                
            }
            Game_Manager.StopSlowMotion();//возвращаемся к нормальному времени
        }
    }
    /// <summary>
    ///  замедляем время, даем возможность менять натяжение
    /// </summary>
    /// <param name="touch">касание</param>
    private void Control(Touch touch)
    {
        //смотрим на фазы касания
        switch (touch.phase)
        {

            case TouchPhase.Began: //при регистрации касания замедляем время
                Game_Manager.StartSlowMotion(10);
                Dragging();
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                Dragging();
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// расчет вектора и силы натяжения, рисование стрелки 
    /// </summary>
    private void Dragging()
    {
        
        touchedWorldPoint = Camera.main.ScreenToWorldPoint(touch.position); //позиция нажатия относительно координат камеры
        pushForceDirection = new Vector2(touchedWorldPoint.x - player.transform.position.x, touchedWorldPoint.y - player.transform.position.y); //расчитываем вектор натяжения
        if (pushForceDirection.sqrMagnitude > maxStretchSqr) // если натяжение больше чем максимально допустимое
        {
            rayToTouchedPoint = new Ray2D(player.transform.position,pushForceDirection); //задаем луч от центра объекта игрока к вектору направления
            touchedWorldPoint = rayToTouchedPoint.GetPoint(maxStretch); //Перезаписываем точку касания в точку по лучу на максимально допустимую дистанцию
        }
        if (pushForceDirection.sqrMagnitude <= 0.66) // если натяжение слишком мало
        {
            ClearLine(); //не рисуем стрелку
            CanJump = false; //не даем возможность прыгнуть
            Debug.DrawLine(touchedWorldPoint, player.transform.position, Color.black);
        }
        else
        {
            CanJump = true;
            Debug.DrawLine(touchedWorldPoint, player.transform.position, Color.red);
            UpdateArrow(); //обновляем стрелку
        }
    }
    /// <summary>
    /// очистка стрелки
    /// </summary>
    private void ClearLine()
    {
        arrow.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero});
    }
    //обновление стрелки
    private void UpdateArrow()
    {
        arrow.SetPosition(0,new Vector2(player.transform.position.x, player.transform.position.y)); //начальная точка от объекта игрока
        arrow.SetPosition(1,new Vector2(touchedWorldPoint.x, touchedWorldPoint.y)); //конечная точка к касанию
    }
}
