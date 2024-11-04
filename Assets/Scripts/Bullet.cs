using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly float _speed = 10f;

    private Vector3 _posInit;

    private const float MaxDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _posInit = transform.position;
        Debug.Log(_posInit);
    }

    // Update is called once per frame
    void Update()
    {
        var transform1 = transform;
        var position = transform1.position;
        position += _speed * Time.deltaTime * transform1.forward;
        transform1.position = position;
        if (Vector3.Distance(_posInit, position) >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }
}
