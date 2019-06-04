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
        spawn();
    }
    void spawn()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < randomIndex; i++)
        {
            float rotate = Random.Range(-90, 90);
            var test = Instantiate(block, spawnPoints[i].position, Quaternion.Euler(0f, 0f, rotate));
            //test.transform.localRotation = ;
        }
    }
}
