using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool Clicked;
    public GameObject hook;
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
    }

    private void OnMouseDown()
    {
        Clicked = true;
        hook = Instantiate(hook, mouseWorldPoint, Quaternion.identity);
    }
    private void OnMouseUp()
    {
        Clicked = false;
    }

    private void testing()
    {
        mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hook.transform.position = mouseWorldPoint;
    }
}
