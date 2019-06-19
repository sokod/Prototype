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
        for (int i = 0; i < randomIndex; i++)
        {
            float rotate = Random.Range(-90, 90);
            Instantiate(block, spawnPoints[i].position, Quaternion.Euler(0f, 0f, rotate));
        }
    }
}
