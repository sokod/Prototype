using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour // Can be deleted!
{
    public GameObject hook_prefab;

    private GameObject spawned_hook;
    private Vector2 touchedWorldPoint; //позиция касания относительно камеры
    private Ray2D rayToTouchedPoint; // луч от центра объекта к точке касания
    private SpringJoint2D spring;
    [Space(15)]
    public float maxStretch = 3.0f;
    public float PercentHead = 0.4f;

    private Touch touch; //касание
    private float maxStretchSqr;
    private GameObject player;
    private bool CanJump;
    private LineRenderer arrow;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spring = player.GetComponent<SpringJoint2D>();
        arrow = player.GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxStretchSqr = maxStretch * maxStretch;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            Control(touch);
        }
    }

    private void Control(Touch touch)
    {
        switch (touch.phase)
        {

            case TouchPhase.Began:
                spawned_hook = Instantiate(hook_prefab, touchedWorldPoint, Quaternion.identity);
                spring.enabled = false;
                Dragging();
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                Dragging();
                break;

            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                Destroy(spawned_hook, 0.5f);
                ClearLine(2);
                if (CanJump)
                {
                    spring.connectedBody = spawned_hook.GetComponent<Rigidbody2D>();
                    spring.enabled = true;
                }
                break;

            default:
                break;
        }
    }

    private void FixedUpdate()
    {
    }


    private void Dragging()
    {
        touchedWorldPoint = Camera.main.ScreenToWorldPoint(touch.position);
        spawned_hook.transform.position = touchedWorldPoint;

        rayToTouchedPoint = new Ray2D(player.transform.position, Vector2.zero);
        Vector2 hookToMouse = new Vector2(touchedWorldPoint.x - player.transform.position.x, touchedWorldPoint.y - player.transform.position.y);
        if (hookToMouse.sqrMagnitude > maxStretchSqr) // если слишком далеко от якоря, то принудетельно ставим на максимально доступное растояние от якоря
        {
            rayToTouchedPoint.direction = hookToMouse;
            touchedWorldPoint = rayToTouchedPoint.GetPoint(maxStretch);
        }
        spawned_hook.transform.position = touchedWorldPoint;

        if (hookToMouse.sqrMagnitude <= 0.66) // если близко к объекту, то не прыгаем
        {
            ClearLine(2);
            CanJump = false;
        }
        else
        {
            CanJump = true;
            UpdateArrow();
        }

    }

    private void ClearLine(int num)
    {
        Vector3[] arr = new Vector3[num];
        arrow.SetPositions(arr);
    }

    private void UpdateArrow()
    {
        arrow.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.4f)
            , new Keyframe(0.999f - PercentHead, 0.4f)  // neck of arrow
            , new Keyframe(1 - PercentHead, 1f)  // max width of arrow head
            , new Keyframe(1, 0f));  // tip of arrow
        arrow.SetPositions(new Vector3[] {
              spawned_hook.transform.position
              , Vector3.Lerp(spawned_hook.transform.position, player.transform.position, 0.999f - PercentHead)
              , Vector3.Lerp(spawned_hook.transform.position, player.transform.position, 1 - PercentHead)
              , player.transform.position });
    }
}

