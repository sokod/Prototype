using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    private SpriteRenderer sprite;
    private void Awake()
    {
        if (gameObject.tag == "Portal")
            StartCoroutine(Glow());
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //если столкнулись с игроком, то делаем блок полупрозрачным. Если он стал полностью прозрачным - уничтожаем.
        if (collision.gameObject.tag == "Player" && gameObject.tag=="Simple Wall")
        {
            sprite.color = new Color(Mathf.Clamp(sprite.color.r+0.3f,0f,1f), Mathf.Clamp(sprite.color.g+0.3f, 0f, 1f), Mathf.Clamp(sprite.color.b+0.3f, 0f, 1f), 1.0f);
            if (sprite.color == Color.white)
                Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Portal")
        {
            UI_Update.Instance.penalty -= 5;
            UI_Update.Instance.forceUpdate = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(Dissapear());
        }
        else if (collision.gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }
        if (collision.gameObject.tag == "Finish" && gameObject.tag=="Portal")
        {
            UI_Update.Instance.penalty += 10;
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
    IEnumerator Glow()
    {
        SpriteRenderer glow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        while (true)
        {
            if (glow.color.a > 0.5f)
            {
                glow.color -= new Color(0f, 0f, 0f, 0.3f);
            }
            else glow.color += new Color(0f,0f,0f,0.05f);
            yield return new WaitForSeconds(0.2f);
        }
    }

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
        StopCoroutine(Glow());
    }
}
