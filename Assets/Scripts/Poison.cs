using System;
using System.Collections;
using System.Collections.Generic;
using PlayerControl;
using UnityEngine;

public class Poison : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Player>(out var p))
        {
            p.OnHit(2.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
