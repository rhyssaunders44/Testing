using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 6;
    public float maxLifetime = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Project();
    }

    public void Project()
    {
        rb.AddForce(transform.up * speed);
        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}