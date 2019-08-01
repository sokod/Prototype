using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject wall;
    public GameObject portal;
    public GameObject oneSideWall;
    public GameObject power_portal;

    private Vector3 lastSpawnedPosition;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnedPosition = new Vector3(0, -3, 0);
        //Spawn();
        AlternateSpawn();
    }

    void AlternateSpawn()
    {
        for (int i = 0; i < 2; i++)
        {
            AlternateCreate();
        }
    }
    void AlternateCreate()
    {
        Vector3 coordinates = FormNewCoordinate();
        Instantiate(power_portal, coordinates, Quaternion.identity);
        lastSpawnedPosition = coordinates;
    }

    Vector3 FormNewCoordinate()
    {
        Vector3 coordinates = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(lastSpawnedPosition.y + 0.5f, lastSpawnedPosition.y + 4f), 0);
        while ((coordinates - lastSpawnedPosition).sqrMagnitude < 2.5f)
        {
            coordinates = FormNewCoordinate();
        }
        return coordinates;
    }

    /// <summary>
    /// спавн блоков
    /// </summary>
    void Spawn()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        float rotate = Random.Range(-90, 90);
        if (randomIndex > spawnPoints.Length / 2)
        {

            Create(randomIndex, rotate, wall);
            rotate = 0;
            Create(randomIndex - 1, rotate, oneSideWall);
        }
        else
        {
            Create(randomIndex, rotate, portal);
            rotate = Random.Range(-90, 90);
            Create(randomIndex + 1, rotate, wall);
        }
    }
    /// <summary>
    /// instantiate объект 
    /// </summary>
    /// <param name="index">позиция</param>
    /// <param name="rotate">наклон</param>
    /// <param name="spawnObject">объект</param>
    /// <returns></returns>
    GameObject Create(int index, float rotate,GameObject spawnObject)
    {
        GameObject spawnedWall;
            spawnedWall = Instantiate(spawnObject, spawnPoints[index].position, Quaternion.Euler(0f, 0f, rotate));
        if (spawnObject.GetComponent<CircleCollider2D>()==null)
            spawnedWall.transform.localScale = new Vector3(Random.Range(4, 10), spawnedWall.transform.localScale.y, spawnedWall.transform.localScale.z);
            //spawnedWall.transform.parent = spawnPoints[index].transform;
        return spawnedWall;
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
