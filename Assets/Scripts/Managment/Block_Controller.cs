using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    private SpriteRenderer sprite;
    private void Awake()
    {
        //если объект портал, то начать свечение
        if (gameObject.tag == "Portal")
            StartCoroutine(Glow());
        //получаем спрайт
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        // если колизия игрока с порталом, то +5 очков, форсим апдейт очков, убираем колайдер портала и включаем корутин.
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Portal")
        {
            UI_Update.Instance.penalty -= 5;
            UI_Update.Instance.forceUpdate = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(Dissapear());
        }
        // если колизия с игроком без уточнения тега объекта, ломаем его.
        else if (collision.gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        // если колизия лавы с порталом, то -6 очков, форсим апдейт очков, уничтожаем портал.
        if (collision.gameObject.tag == "Finish" && gameObject.tag=="Portal")
        {
            UI_Update.Instance.penalty += 6;
            UI_Update.Instance.forceUpdate = true;
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
    /// динамическая смена компонента альфа в цвете (0.2 - 0.5).
    /// </summary>
    /// <returns></returns>
    IEnumerator Glow()
    {
        // получаем компонент
        SpriteRenderer glow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        //меняем альфу пока принудительно не остановим корутин
        while (true)
        {
            if (glow.color.a > 0.8f)
            {
                glow.color -= new Color(0f, 0f, 0f, 0.3f);
            }
            else glow.color += new Color(0f,0f,0f,0.05f);
            yield return new WaitForSeconds(0.2f);
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
