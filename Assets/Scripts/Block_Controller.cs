using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    private SpriteRenderer sprite;
    private void Awake()
    {
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
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //если столкнулись с полом, то уничтожаем себя.
        if (collision.gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }
}
