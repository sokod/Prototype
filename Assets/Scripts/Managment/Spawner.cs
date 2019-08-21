using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3 lastSpawnedPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnedPosition = new Vector3(0, -3, 0);
        AlternateSpawn();
    }

    void AlternateSpawn()
    {
        for (int i = 0; i < 4; i++)
        {
            AlternateCreate();
        }
    }
    void AlternateCreate()
    {
        Vector3 coordinates = FormNewCoordinate();
        Instantiate(Game_Loader.Instance.blocks[1].block, coordinates, Quaternion.identity);
        lastSpawnedPosition = coordinates;
    }

    Vector3 FormNewCoordinate()
    {
        Vector3 coordinates = new Vector3(Random.Range(-2.25f, 2.25f), Random.Range(lastSpawnedPosition.y + 0.5f, lastSpawnedPosition.y + 4f), 0);
        while ((coordinates - lastSpawnedPosition).sqrMagnitude < 2.5f)
        {
            coordinates = FormNewCoordinate();
        }
        return coordinates;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            transform.position += Vector3.up*3.5f;
            //Spawn();
            AlternateSpawn(); 
        }
    }
}
