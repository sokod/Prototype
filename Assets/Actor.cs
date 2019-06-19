﻿using UnityEngine;

public class Actor : MonoBehaviour
{
    private Player_Controller controller;
    private SpriteRenderer sprite;
    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Player_Controller>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Simple Wall")
        {
            controller.ResetJumps();
        }
    }
    private void Update()
    {
        if (!controller.CanJump())
        {
            sprite.color = Color.black;
        }
        else
        {
            sprite.color = Color.yellow;
        }
    }
}
