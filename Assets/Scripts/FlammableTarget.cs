using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlammableTarget : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    private GameObject _child;
    // Start is called before the first frame update
    private void Start()
    {
        _child = transform.GetChild(0).gameObject;
        _child.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent<Bullet>(out _)) return;
        _child.SetActive(true);
        Destroy(gameObject, 1f);
    }
}
