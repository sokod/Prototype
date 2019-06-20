using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Actor : MonoBehaviour
{
    private Player_Controller controller;
    private SpriteRenderer sprite;
    public GameObject deathFloor;
    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Player_Controller>();
        sprite = GetComponent<SpriteRenderer>();

    }
    private void FixedUpdate()
    {
    }

    private void Update()
    {
        if (!controller.CanJump())
        {
            sprite.color = new Color(1f, 0.92f, 0.6f, 1f);
        }
        else
        {
            sprite.color = Color.yellow;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Simple Wall")
        {
            controller.ResetJumps();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
