using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float Speed = 20f;

    private Vector3 _posInit;

    private const float MaxDistance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _posInit = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        var transform1 = transform;
        var position = transform1.position;
        position += Speed * Time.deltaTime * transform1.forward;
        transform1.position = position;
        if (Vector3.Distance(_posInit, position) >= MaxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.GetHit(10f);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        //Debug.Log("Destroyed");
    }
}
