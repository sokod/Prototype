using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Portal : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        //если объект портал, то начать свечение
            animator = GetComponent<Animator>();
            StartCoroutine(Glow());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // если колизия игрока с порталом, то +5 очков, форсим апдейт очков, убираем колайдер портала и включаем корутин.
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collision with player. gameObject - " + name + " " + tag);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            animator.SetBool("PlayerCollision", true);
        }
        // если колизия лавы с порталом, то -3 очков, форсим апдейт очков, уничтожаем портал.
        if (collision.gameObject.tag == "Finish"&& animator.isActiveAndEnabled)
        {

                Debug.Log("Collision with deathFloor. gameObject - " + name + " " + tag);
                Destroy(gameObject);
        }

        //after expansion check for collisions with surroundings and change them with canJump portal;
        if (collision.gameObject.tag == "Simple Wall" || collision.gameObject.tag == "One Side Wall")
        {
            Vector3 position = collision.gameObject.transform.position;
            Debug.Log("Found object for substitude " + collision.gameObject.name);
            Destroy(collision.gameObject);
            Instantiate(Game_Loader.Instance.blocks[1].block, position, Quaternion.identity);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //если столкнулись с полом, то уничтожаем себя.
        if (collision.gameObject.tag == "Finish" && animator.isActiveAndEnabled)
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
            if (glow.color.a < 0.8f && brignting)
            {
                glow.color += new Color(0f, 0f, 0f, 0.05f);
            }
            else if (glow.color.a > 0.8f || !brignting)
            {
                brignting = false;
                glow.color -= new Color(0f, 0f, 0f, 0.05f);
            }
            if (glow.color.a < 0.4f)
            {
                brignting = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestroyPortal()
    {
        animator.enabled = false;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //останавливаем корутин свечения
        StopCoroutine(Glow());
    }
}
