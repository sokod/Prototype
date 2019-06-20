using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Actor : MonoBehaviour
{
    private Player_Controller controller;
    private SpriteRenderer sprite;
    public GameObject deathFloor;
    public ParticleSystem collisionParticle;
    private Rigidbody2D player_rb;

    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Player_Controller>();
        sprite = GetComponent<SpriteRenderer>();
        player_rb = GetComponent<Rigidbody2D>();

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
            if (player_rb.velocity.magnitude > 3) //просчитываем примерную силу столкновения
            {
                collisionParticle.transform.position = collision.collider.ClosestPoint(transform.position); //узнаем примерную точку столкновения
                Vector3 direction = transform.position - collision.transform.position; //узнаем вектор направления к объекту колизии
                direction.Normalize();
                Game_Manager.SetAngle(direction, collisionParticle.transform); // поворачиваем систему частиц к направлению колизии
                collisionParticle.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //рестарт
        }
    }
}
