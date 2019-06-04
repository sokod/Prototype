using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D hook_rb;
    public Transform hook_transform;
    [Space(15)]
    public float maxStretch = 3.0f;

    private Rigidbody2D player_rb;
    private Ray2D rayToMouse;
    private float maxStretchSqr;
    private SpringJoint2D spring;
    private Vector2 velocity, prevVelocity;
    private bool CanJump,Clicked = false;

    void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
        player_rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        maxStretchSqr = maxStretch * maxStretch;
    }

    // Update is called once per frame
    void Update()
    {
        if (spring.enabled)
        {
            if (prevVelocity.sqrMagnitude > player_rb.velocity.sqrMagnitude)
            {
                spring.enabled = false;
                player_rb.velocity = prevVelocity;
            }
            if (!Clicked) prevVelocity = player_rb.velocity;
        }
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
    }
    private void OnMouseUp()
    {
        prevVelocity = player_rb.velocity;
        Clicked = false;
        if (CanJump)
            spring.enabled = true;

    }
    private void Dragging()
    {
        rayToMouse = new Ray2D(transform.position, Vector2.zero);
        Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 hookToMouse = new Vector2(mouseWorldPoint.x - transform.position.x, mouseWorldPoint.y - transform.position.y);
        if (hookToMouse.sqrMagnitude>maxStretchSqr)
        {
            rayToMouse.direction = hookToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }
        hook_transform.position = mouseWorldPoint;
        if (hookToMouse.sqrMagnitude <= 0.25)
        {
            CanJump = false;
        }
        else CanJump = true;
    }
}
