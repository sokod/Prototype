using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Controller : MonoBehaviour
{
    public GameObject hook_prefab;

    private GameObject spawned_hook;
    private Vector2 mouseWorldPoint;
    private Ray2D rayToMouse;
    private SpringJoint2D spring;
    [Space(15)]
    public float maxStretch = 3.0f;
    private float maxStretchSqr;
    private GameObject player;
    private bool CanJump, Clicked;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spring = player.GetComponent<SpringJoint2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        maxStretchSqr = maxStretch * maxStretch;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Clicked)
            Dragging();
        else
        {
        }
    }

    public GameObject ReturnHook()
    {
        return spawned_hook;
    }

    private void OnMouseDown()
    {
        Clicked = true;
        spawned_hook = Instantiate(hook_prefab, mouseWorldPoint, Quaternion.identity);
        spring.enabled = false;
    }
    private void OnMouseUp()
    {
        Clicked = false;
        Destroy(spawned_hook,1f);

        if (CanJump)
            spring.connectedBody = spawned_hook.GetComponent<Rigidbody2D>();
            spring.enabled = true;
    }

    private void Dragging()
    {
        mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawned_hook.transform.position = mouseWorldPoint;

        rayToMouse = new Ray2D(player.transform.position, Vector2.zero);
        Vector2 hookToMouse = new Vector2(mouseWorldPoint.x - player.transform.position.x, mouseWorldPoint.y - player.transform.position.y);
        if (hookToMouse.sqrMagnitude > maxStretchSqr) // если слишком далеко от якоря, то принудетельно ставим на максимально доступное растояние от якоря
        {
            rayToMouse.direction = hookToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }
        spawned_hook.transform.position = mouseWorldPoint;

        if (hookToMouse.sqrMagnitude <= 0.66) // если близко к объекту, то не прыгаем
        {
            CanJump = false;
        }
        else CanJump = true;

    }
}
