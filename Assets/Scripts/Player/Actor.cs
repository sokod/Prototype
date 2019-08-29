using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private Player_Controller controller;
    private ParticleSystem collisionParticle;
    private Rigidbody2D player_rb;

    private void Awake()
    {
        controller = GetComponent<Player_Controller>();
        player_rb = GetComponent<Rigidbody2D>();
        collisionParticle = GameObject.FindGameObjectsWithTag("Particle System")[1].GetComponent<ParticleSystem>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Simple Wall")
        {
            controller.ResetJumps();
            SetCollisionParticles(collision);
        }
        if (collision.gameObject.tag == "One Side Wall" && collision.relativeVelocity.y>=0f) //если был выше во время столкновения
        {
            controller.ResetJumps();
            SetCollisionParticles(collision);
        }

    }
    /// <summary>
    /// настройка цвета колизии и включение эффекта.
    /// </summary>
    /// <param name="collision"></param>
    private void SetCollisionParticles(Collision2D collision)
    {
        if (player_rb.velocity.magnitude > 3) //просчитываем примерную силу столкновения
        {
            //collisionParticle.transform.position = collision.collider.ClosestPoint(transform.position); //узнаем примерную точку столкновения
            collisionParticle.transform.position = collision.contacts[0].point; //узнаем примерную точку столкновения
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
        //если столкнулись с лавой, включаем экран смерти, обновляем очки, начинаем замедление времени
        if (collision.gameObject.tag == "Finish")
        {
            UI_Update.Instance.DeadScreen.SetActive(true);
            UI_Update.Instance.ShowPauseButton(false);
            UI_Update.Instance.UpdateHighScore();
            Game_Loader.Instance.UpdateGems(Mathf.RoundToInt(UI_Update.Instance.score/10f));
            UI_Update.Instance.HighestScore.text = string.Format($"Highest score: {PlayerPrefs.GetFloat("HighScore", 0):f0}");
            Game_Manager.Instance.StartSlowMotion(20);
            StartCoroutine(GameOver());
        }
        if (collision.gameObject.tag == "Simple Wall") //желтый портал, эксперементально
        {
            controller.ResetJumps();
        }
    }
    /// <summary>
    /// пауза после 5 секунд
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(1.25f);
        UI_Update.Instance.Pause();
    }

}
