using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    private SpriteRenderer sprite;
    private void Awake()
    {
        if (gameObject.tag == "Portal")
            StartCoroutine(Fade());
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
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Finish" && gameObject.tag=="Portal")
        {
            UI_Update.Instance.penalty += 10;
            UI_Update.Instance.forceUpdate = true;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player" && gameObject.tag=="Portal")
        {
            UI_Update.Instance.penalty -= 5;
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
    IEnumerator Fade()
    {
        SpriteRenderer glow = gameObject.GetComponentsInChildren<SpriteRenderer>()[1];
        while (true)
        {
            Debug.Log(glow.color);
            if (glow.color.a > 0.45f) glow.color -= new Color(0f,0f,0f,0.3f);
            glow.color += new Color(0f,0f,0f,0.05f);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnDestroy()
    {
        StopCoroutine(Fade());
    }
}
