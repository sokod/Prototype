using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject wall;
    public GameObject oneSideWall;

    private GameObject spawnedWall;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
            float rotate = Random.Range(-90, 90);

        if (randomIndex > spawnPoints.Length / 2)
        {
            spawnedWall = Instantiate(wall, spawnPoints[randomIndex].position, Quaternion.Euler(0f, 0f, rotate));
            spawnedWall.transform.localScale = new Vector3(Random.Range(4, 10), spawnedWall.transform.localScale.y, spawnedWall.transform.localScale.z);
            rotate = Random.Range(-90, 90);
            spawnedWall = Instantiate(oneSideWall, spawnPoints[randomIndex-1].position, Quaternion.Euler(0f, 0f, rotate));
            spawnedWall.transform.localScale = new Vector3(Random.Range(4, 10), spawnedWall.transform.localScale.y, spawnedWall.transform.localScale.z);
        }
        else
        {
            spawnedWall = Instantiate(wall, spawnPoints[randomIndex+1].position, Quaternion.Euler(0f, 0f, rotate));
            spawnedWall.transform.localScale = new Vector3(Random.Range(4, 10), spawnedWall.transform.localScale.y, spawnedWall.transform.localScale.z);
            rotate = Random.Range(-90, 90);
            spawnedWall = Instantiate(oneSideWall, spawnPoints[randomIndex].position, Quaternion.Euler(0f, 0f, rotate));
            spawnedWall.transform.localScale = new Vector3(Random.Range(4, 10), spawnedWall.transform.localScale.y, spawnedWall.transform.localScale.z);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            transform.position += Vector3.up*3.7f;
            Spawn();
        }
    }
}
