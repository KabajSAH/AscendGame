using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flying : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private Vector3 finishPos;

    private float _timeToWait;

    [SerializeField] private float timeToComplete;
    [SerializeField] private float maxWaiting;
    
    private float _time;
    private bool _movingToTarget = true;

    private void Start()
    {
        gameObject.transform.position = startingPos;
        _time = 0f;
        _timeToWait = 0f;
    }

    private void Update()
    {
        // Calcul du déplacement de la plateforme
        var currentPosition = transform.position;

        if (Vector3.Distance(transform.position, startingPos) < 0.5f 
            || Vector3.Distance(transform.position, finishPos) < 0.5f)
        {
            _timeToWait += Time.deltaTime;
        }
    
        if (_timeToWait >= maxWaiting)
        {
            if (_movingToTarget)
            {
                transform.position = Vector3.Lerp(startingPos, finishPos, _time);
                _time += Time.deltaTime / timeToComplete;
            }
            else
            {
                transform.position = Vector3.Lerp(finishPos, startingPos, _time);
                _time += Time.deltaTime / timeToComplete;
            }
            _timeToWait = maxWaiting;
        }

        if (!(_time >= 1)) return;
        _movingToTarget = !_movingToTarget;
        _timeToWait = 0f;
        _time = 0;
    }

    private void OnCollisionStay(Collision other)
    {
        // Si l'objet en collision a un Rigidbody, on ajuste son parent
        if (!other.gameObject.TryGetComponent(out Rigidbody _)) return;
        
        other.transform.SetParent(transform);
    }
    
    private void OnCollisionExit(Collision other)
    {
        // Supprime le parentage lorsque l'objet quitte la plateforme
        if (!other.gameObject.TryGetComponent(out Rigidbody _)) return;
        other.transform.SetParent(null);
    }
}
