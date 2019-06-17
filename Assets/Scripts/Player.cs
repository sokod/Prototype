using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject hook_transform;
    [Space(15)]
    public float maxStretch = 3.0f;

    private Rigidbody2D player_rb;
    private Ray2D rayToMouse;
    private float maxStretchSqr;
    private SpringJoint2D spring;
    private bool CanJump,Clicked;
    private Test scriptInstance = null;

    void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        player_rb = GetComponent<Rigidbody2D>();
        GameObject tempObj = GameObject.Find("Touch Screen");
        scriptInstance = tempObj.GetComponent<Test>();
    }

    private void Start()
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
    }

    private void OnMouseDown()
    {

        Clicked = true;
        spring.enabled = false;
        hook_transform = scriptInstance.ReturnHook();
        //hook_transform = GetComponent("Hook(Clone)") as Transform;
    }
    private void OnMouseUp()
    {
        Clicked = false;
        if (CanJump)
            spring.enabled = true;

    }
    private void Dragging()
    {
        rayToMouse = new Ray2D(transform.position, Vector2.zero);
        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 hookToMouse = new Vector2(mouseWorldPoint.x - transform.position.x, mouseWorldPoint.y - transform.position.y);
        if (hookToMouse.sqrMagnitude>maxStretchSqr) // если слишком далеко от якоря, то принудетельно ставим на максимально доступное растояние от якоря
        {
            rayToMouse.direction = hookToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }
        hook_transform.transform.position = mouseWorldPoint;
        
        if (hookToMouse.sqrMagnitude <= 0.66) // если близко к объекту, то не прыгаем
        {
            CanJump = false;
        }
        else CanJump = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spring.enabled = false;
        Debug.Log("Collision with " + collision.transform.name);
    }

}
