using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    private SpriteRenderer sprite;
    private void Awake()
    {
        //если объект портал, то начать свечение
        if (gameObject.GetComponentsInChildren<SpriteRenderer>().Length>1)
        {
            StartCoroutine(Glow());
        }
        //получаем спрайт
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " entered collision with " + collision.gameObject.name);
        //если столкнулись с игроком, то делаем блок полупрозрачным. Если он стал полностью прозрачным - уничтожаем.
        if (collision.gameObject.tag == "Player" && gameObject.tag=="Simple Wall")
        {
            sprite.color = new Color(Mathf.Clamp(sprite.color.r+0.3f,0f,1f), Mathf.Clamp(sprite.color.g+0.3f, 0f, 1f), Mathf.Clamp(sprite.color.b+0.3f, 0f, 1f), 1.0f);
            if (sprite.color == Color.white)
            {
                sprite.color = new Color(1f, 1f, 1f, 0f);
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<ParticleSystem>().Play();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " entered trigger with " + collision.gameObject.name);
        // если колизия игрока с порталом, то +5 очков, форсим апдейт очков, убираем колайдер портала и включаем корутин.
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Portal")
        {
            UI_Update.Instance.penalty -= 2;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(Dissapear());
        }
        // если колизия лавы с порталом, то -3 очков, форсим апдейт очков, уничтожаем портал.
        if (collision.gameObject.tag == "Finish" && gameObject.tag=="Portal")
        {
            UI_Update.Instance.penalty += 1;
            Debug.Log("Collision with deathFloor. gameObject - " + name + " " + tag);
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //если столкнулись с полом, то уничтожаем себя.
        if (collision.gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// динамическая смена компонента альфа в цвете (0.4 - 0.8).
    /// </summary>
    /// <returns></returns>
    IEnumerator Glow()
    {
        // получаем компонент
        SpriteRenderer glow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        bool brignting = true;
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        //меняем альфу пока принудительно не остановим корутин
        while (true)
        {
            if (glow.color.a<0.8f && brignting)
            {
                glow.color += new Color(0f, 0f, 0f, 0.05f);
            }
            else if (glow.color.a > 0.8f || !brignting)
            {
                brignting = false;
                glow.color -= new Color(0f, 0f, 0f, 0.05f);
            }
            if (glow.color.a<0.4f)
            {
                brignting = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// уменшаем объект до 0.1 и затем уничтожаем его
    /// </summary>
    /// <returns></returns>
    IEnumerator Dissapear()
    {
        while (transform.localScale.x>0.1f)
        {
            transform.localScale -= new Vector3(0.1f, 0.1f, 0f);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //останавливаем корутин свечения
        StopCoroutine(Glow());
    }
}
