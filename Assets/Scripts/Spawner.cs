using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject block;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
            float rotate = Random.Range(0, 180);
            Debug.Log(rotate);
            Instantiate(block, spawnPoints[randomIndex].position, Quaternion.Euler(0f, 0f, rotate));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            transform.position += Vector3.up*5f;
            Spawn();
        }
    }
}
