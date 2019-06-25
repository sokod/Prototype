using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private Player_Controller controller;
    private SpriteRenderer sprite;

    
    public GameObject deathFloor;
    
    public ParticleSystem collisionParticle;

    private Rigidbody2D player_rb;

    private void Awake()
    {
        controller = GetComponent<Player_Controller>();
        //controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Player_Controller>();
        sprite = GetComponent<SpriteRenderer>();
        player_rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (!controller.CanJump())
        {
            sprite.color = new Color(1f, 0.92f, 0.7f, 1f);
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
            SetCollisionParticles(collision);
        }
        if (collision.gameObject.tag == "One Side Wall" && collision.relativeVelocity.y>=0f)
        {
            controller.ResetJumps();
            SetCollisionParticles(collision);
        }

    }

    private void SetCollisionParticles(Collision2D collision)
    {
        if (player_rb.velocity.magnitude > 3) //просчитываем примерную силу столкновения
        {
            //collisionParticle.transform.position = collision.collider.ClosestPoint(transform.position); //узнаем примерную точку столкновения
            collisionParticle.transform.position = collision.contacts[0].point; //узнаем примерную точку столкновения
            //Vector3 direction = transform.position - collision.transform.position; //узнаем вектор направления к объекту колизии
            //direction.Normalize();
            //Game_Manager.Instance.SetAngle(direction, collisionParticle.transform); // поворачиваем систему частиц к направлению колизии

            SpriteRenderer collisionObject = collision.collider.GetComponent<SpriteRenderer>();

            // установка цвета градиента в цикле жизни частицы
            var col = collisionParticle.colorOverLifetime;
            Gradient gradient = new Gradient();
            gradient.SetKeys( //обновляем цвет стрелки, чем больше сила - тем ярче цвет.
                    new GradientColorKey[] { new GradientColorKey(collisionObject.color, 0f), new GradientColorKey(collisionObject.color, 1f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
                    );
            col.color = gradient;
     
            collisionParticle.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Game_Manager.Instance.Restart();
        }
        if (collision.gameObject.tag == "Simple Wall")
        {
            controller.ResetJumps();
        }
    }
}
