using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableTarget : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent<Bullet>(out _)) return;
        Debug.Log(other);
        Destroy(gameObject);
    }
}
