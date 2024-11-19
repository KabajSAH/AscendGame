using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flying : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private Vector3 finishPos;

    private float _timeToWait;

    [SerializeField] private float maxWaiting;
    
    private float _time;
    private bool _movingToTarget = true;
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.transform.position = startingPos;
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
        }

        if (_timeToWait >= maxWaiting)
        {
            if (_movingToTarget)
            {
                transform.position = Vector3.Lerp(startingPos, finishPos, _time);
                _time += Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.Lerp( finishPos, startingPos, _time);
                _time += Time.deltaTime;
            }

            _timeToWait = maxWaiting;
        }

        if (!(_time >= 1)) return;
        _movingToTarget = !_movingToTarget;
        _timeToWait = 0f;
        _time = 0;

    }
}
