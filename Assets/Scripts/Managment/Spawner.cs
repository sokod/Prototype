using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3 lastSpawnedPosition;
    private int difficulty = 0;
    private float Y_position;
    public float rangeModificator=15f;

    // Start is called before the first frame update
    void Start()
    {
        Y_position = gameObject.transform.position.y;
        lastSpawnedPosition = new Vector3(0, -3, 0);
    }

    void Spawn()
    {
        while (lastSpawnedPosition.y < Y_position + 2f)
        {
            CreateObject();
        }
        if ((Random.Range(0, 100) + difficulty) > 75) CreateSupportBlock();
    }
    void CreateObject()
    {
        Vector3 coordinates = FormNewCoordinate(difficulty);
        GameObject spawned = Instantiate(GetMainBlockObject(), coordinates, Quaternion.identity);
        SetTransform(spawned.transform);
        lastSpawnedPosition = coordinates;
    }

    private void CreateSupportBlock()
    {
        int value = Random.Range(0, 10);
        Vector3 coordinates = FormNewCoordinate((int)-rangeModificator);
        lastSpawnedPosition = coordinates;
        if (value < 4) Instantiate(Game_Loader.Instance.blocks[4].block, coordinates, Quaternion.identity); // portal with points
        else if (value > 4 && value < 8) Instantiate(Game_Loader.Instance.blocks[5].block, coordinates, Quaternion.identity); // substitution portal
        else Instantiate(Game_Loader.Instance.blocks[2].block, coordinates, Quaternion.identity); // gems
    }

    /// <summary>
    /// Возвращаем объект для спавна в зависимости от текущей сложности и удачи
    /// </summary>
    /// <returns></returns>
    GameObject GetMainBlockObject()
    {
        difficulty = (int)Game_Manager.Instance.playerMoveScore/10;
        int randValue = Random.Range(0, 1000);
        
        #region Difficulty_0
        if (difficulty == 0 && randValue < 800) { return Game_Loader.Instance.blocks[1].block; } // can jump portal;
            else if (difficulty == 0 && randValue > 800 && randValue < 900) { return Game_Loader.Instance.blocks[0].block; } // simple block
            else if (difficulty == 0 && randValue > 900) {return Game_Loader.Instance.blocks[3].block; } // one side wall
        #endregion
        randValue += difficulty*2;
        if (randValue<300)
        {return Game_Loader.Instance.blocks[1].block; } // can jump portal;
            else if (randValue>300 && randValue<600)
            { return Game_Loader.Instance.blocks[0].block; } // simple block;
            else if (randValue>600 && randValue<1000)
            { return Game_Loader.Instance.blocks[3].block; } // one side wall
                else { return Game_Loader.Instance.blocks[1].block; } // can jump portal;
    }

    void SetTransform(Transform transform)
    {
        transform.parent = gameObject.transform.parent;
        if (transform.tag == "Simple Wall" && !transform.GetComponent<CircleCollider2D>())
        {
            if (transform.position.x > 1) transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 140f));
            else if (transform.position.x < 1) transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, -140f));
            transform.localScale = new Vector3(Random.Range(3f, 5f), Random.Range(1f, 1.5f), 0f);
        }
        else if (transform.tag == "One Side Wall")
        {
            if (transform.position.x > 1) transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 25f));
            else if (transform.position.x < 1) transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, -25f));
            transform.localScale = new Vector3(Random.Range(3f, 5f), Random.Range(1f, 1.5f), 0f);
        }
        
    }

    Vector3 FormNewCoordinate(int difficulty)
    {
        float minY = Mathf.Clamp((Random.Range(0.5f, 1.5f) * (difficulty+rangeModificator)/rangeModificator), 0.5f, 4f);
        Vector3 coordinates = new Vector3(Random.Range(-2.25f, 2.25f), lastSpawnedPosition.y + minY, 0);
        while ((coordinates - lastSpawnedPosition).sqrMagnitude < 2.5f)
        {
            coordinates = FormNewCoordinate(difficulty);
        }
        Debug.LogWarning(coordinates.y);
        return coordinates;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Spawning new objects");
        if (collision.gameObject.tag == "MainCamera")
        {
            transform.position += Vector3.up*3.5f;
            Y_position = transform.position.y;
            Spawn(); 
        }
    }
}
