using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private CharacterController _controller;
    [SerializeField] private float jumpStrength = 5;
    [SerializeField] private float speed = 5;
    public const int MaxHealth = 100;
    public float health;

    private bool _haveFire;

    private bool _haveEarth;

    private FirePower _firePower;

    private EarthPower _earthPower;
    // Start is called before the first frame update
    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _controller = GetComponent<CharacterController>();
        _earthPower = GetComponent<EarthPower>();
        _firePower = GetComponent<FirePower>();
        health = 75f;
    }

    // Update is called once per frame
    private void Update()
    {
        var moveValue = new Vector3(_moveAction.ReadValue<Vector2>().x,0 ,_moveAction.ReadValue<Vector2>().y);
        _controller.Move(moveValue * (speed * Time.deltaTime));
        
        var jumpValue = _jumpAction.ReadValue<float>();
        if ( Math.Abs(jumpValue - 1) < 0.001f ) _controller.Move(Vector3.up * (jumpStrength * Time.deltaTime));
        
        if (Input.GetKey(KeyCode.L)) _haveFire = true;

        if (Input.GetKey(KeyCode.G)) _haveEarth = true;
        
        if (_haveFire && !_firePower.enabled) _firePower.enabled = true;
        
        if (_haveEarth && !_earthPower.enabled) _earthPower.enabled = true;
        
        if (health <= 0) Death();
    }

    private void Death()
    {
        Destroy(this);
    }
    
    
}
