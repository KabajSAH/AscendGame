using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class EarthPower : MonoBehaviour
{
    private float _shield;
    private const int MaxShield = 500;
    private float _timeTillEnd;
    private const int 
    private float _passiveShield;
    private const int MaxPassive = 20;
    private float _timeTillRegen;
    private const int Regen = 2;
    private float _timeTillCooldown;
    private const int Cooldown = 5;
    private InputAction _shieldAction;
    [SerializeField] private GameObject objectShield;
    
    // Start is called before the first frame update
    private void Start()
    {
        _shield = MaxShield;
        _passiveShield = 10f;
        _timeTillCooldown = Cooldown;
        _shieldAction = InputSystem.actions.FindAction("Shield");
    }

    // Update is called once per frame
    private void Update()
    {
        var shieldValue = _shieldAction.ReadValue<float>();
        if (Math.Abs(shieldValue - 1) < 0.001f && _timeTillCooldown > Cooldown)
        {
            objectShield.SetActive(true);
        }

        if (_passiveShield < MaxPassive && _timeTillRegen >= Regen)
        {
            _passiveShield += 5f * Time.deltaTime;
        }

        if (_shield <= 0 )
        {
            objectShield.SetActive(false);
            _timeTillCooldown = 0;
            _shield = MaxShield;
        }
    }
}
