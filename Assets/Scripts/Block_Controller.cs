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
        if (collision.gameObject.tag == "Player")
        {
            sprite.color -= new Color(0f, 0f, 0f, 0.5f);
            if (sprite.color.a<=0)
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
