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
    private const int TimeActive = 5;
    public float passiveShield;
    public const int MaxPassive = 20;
    private float _timeTillRegen;
    private const int Regen = 2;
    private float _timeTillCooldown;
    private const int Cooldown = 5;
    private float _previousHp;
    private InputAction _shieldAction;
    [SerializeField] private GameObject objectShield;
    private Player _player;
    
    // Start is called before the first frame update
    private void Start()
    {
        _shield = MaxShield;
        passiveShield = 10f;
        _timeTillCooldown = Cooldown;
        _shieldAction = InputSystem.actions.FindAction("Shield");
        _player = GetComponent<Player>();
        _previousHp = _player.health;
    }

    // Update is called once per frame
    private void Update()
    {
        // Effet passif du pouvoir
        if (_player.health < _previousHp) _timeTillRegen = 0f;
        else _timeTillRegen += Time.deltaTime;
        
        if (passiveShield < MaxPassive && _timeTillRegen >= Regen)
        {
            passiveShield += 5f * Time.deltaTime;
            _timeTillRegen = Regen;
        }
        _previousHp = _player.health;
        if (passiveShield > MaxPassive) passiveShield = MaxPassive;
        
        // Effet actif du pouvoir
        var shieldValue = _shieldAction.ReadValue<float>();
        if (Math.Abs(shieldValue - 1) < 0.001f && _timeTillCooldown >= Cooldown)
        {
            objectShield.SetActive(true);
            _shield = MaxShield;
            _timeTillCooldown = Cooldown;
        }

        if (objectShield.activeSelf) _timeTillEnd += Time.deltaTime;
        
        if (_shield <= 0 || _timeTillEnd >= TimeActive)
        {
            objectShield.SetActive(false);
            _timeTillCooldown = 0f;
            _timeTillEnd = 0f;
        }

        if (_timeTillCooldown < Cooldown) _timeTillCooldown += Time.deltaTime;
        
    }
}
