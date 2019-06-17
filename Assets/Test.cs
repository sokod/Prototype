using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool Clicked;
    public GameObject hook;
    private GameObject spawnedHook;
    private Vector2 mouseWorldPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Clicked)
            testing();
        else
        {
        }
    }

    public GameObject ReturnHook()
    {
        return spawnedHook;
    }

    private void OnMouseDown()
    {
        Clicked = true;
        spawnedHook = Instantiate(hook, mouseWorldPoint, Quaternion.identity);
    }
    private void OnMouseUp()
    {
        Clicked = false;
        Destroy(spawnedHook,1f);
    }

    private void testing()
    {
        mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnedHook.transform.position = mouseWorldPoint;
    }
}
