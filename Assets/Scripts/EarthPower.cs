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
    public float timeTillCooldown;
    public const int Cooldown = 5;
    private float _previousShield;
    private InputAction _shieldAction;
    [SerializeField] private GameObject objectShield;
    private Animator _animator;
    private float _lastValue; // Stocke la dernière valeur
    private float _timeSinceLastDecrease; // Temps écoulé depuis la dernière réduction
    private static readonly int Earth = Animator.StringToHash("Earth");


    // Start is called before the first frame update
    private void Start()
    {
        _shield = MaxShield;
        passiveShield = 0f;
        timeTillCooldown = Cooldown;
        _shieldAction = InputSystem.actions.FindAction("Shield");
        _previousShield = passiveShield;
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        // Effet passif du pouvoir
        if (passiveShield < _previousShield) _timeTillRegen = 0f;
        else
        {
            _timeTillRegen += Time.deltaTime;
            if (passiveShield < MaxPassive && _timeTillRegen >= Regen)
            { 
                passiveShield += 5f * Time.deltaTime; 
                _timeTillRegen = Regen;
            }
        }
        
        if (passiveShield > MaxPassive) passiveShield = MaxPassive;
        _previousShield = passiveShield;
        // Effet actif du pouvoir
        var shieldValue = _shieldAction.ReadValue<float>();
        if (Math.Abs(shieldValue - 1) < 0.001f && timeTillCooldown >= Cooldown)
        {
            objectShield.SetActive(true);
            _animator.SetBool(Earth, true);
            _shield = MaxShield;
            timeTillCooldown = Cooldown;
        }

        if (objectShield.activeSelf) _timeTillEnd += Time.deltaTime;
        
        if (_shield <= 0 || _timeTillEnd >= TimeActive)
        {
            objectShield.SetActive(false);
            _animator.SetBool(Earth, false);
            timeTillCooldown = 0f;
            _timeTillEnd = 0f;
        }

        if (timeTillCooldown < Cooldown) timeTillCooldown += Time.deltaTime;
        
    }
}
