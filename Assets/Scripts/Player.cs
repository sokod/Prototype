using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpringJoint2D spring;

    private void Awake()
    {
        spring = GetComponent<SpringJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spring.enabled = false;
        Debug.Log("Collision with " + collision.transform.name);
    }

}
