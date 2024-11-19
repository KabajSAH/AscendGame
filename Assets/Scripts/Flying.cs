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

    private Vector3 _displacement;
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.transform.position = startingPos;
        _displacement = Vector3.zero;
        _time = 0f;
        _timeToWait = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector3.Distance(transform.position, startingPos) < 0.1f 
            || Vector3.Distance(transform.position, finishPos) < 0.1f)
        {
            _timeToWait += Time.deltaTime;
            _displacement = Vector3.zero;
        }
        
        if (_timeToWait >= maxWaiting)
        {
            if (_movingToTarget)
            {
                _displacement =( Vector3.Lerp(startingPos, finishPos, _time) - transform.position) * 5f;
                transform.position = Vector3.Lerp(startingPos, finishPos, _time);
                _time += Time.deltaTime / timeToComplete;
            }
            else
            {
                _displacement = (Vector3.Lerp(finishPos,startingPos, _time) - transform.position)*5f;
                transform.position = Vector3.Lerp( finishPos, startingPos, _time);
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
        if (other.gameObject.TryGetComponent<Player>(out var player))
        {
            Debug.Log(_displacement);
            player.transform.position += _displacement;
        }
    }
    
}
