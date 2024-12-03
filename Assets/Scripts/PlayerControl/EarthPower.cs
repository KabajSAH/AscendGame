using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
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
        public const int Cooldown = 10;
    
        private float _previousShield;
        private InputAction _shieldAction;
        private const float ShieldSpeed = 4f;
        private float _speed;
        [SerializeField] private GameObject objectShield;
        private Player _player;
        private float _timeSinceLastDecrease; // Temps écoulé depuis la dernière réduction
    
        private static readonly int Earth = Animator.StringToHash("Earth");


        // Start is called before the first frame update
        private void Start()
        {
            _player = GetComponent<Player>();
            _shield = MaxShield;
            _speed = _player.Speed;
            objectShield.SetActive(false);
            passiveShield = 0f;
            timeTillCooldown = Cooldown;
            _shieldAction = InputSystem.actions.FindAction("Shield");
            _previousShield = passiveShield;
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
                _player.Animator.SetBool(Earth, true);
                _player.Speed = ShieldSpeed;
                _shield = MaxShield;
                timeTillCooldown = Cooldown;
            }

            if (objectShield.activeSelf) _timeTillEnd += Time.deltaTime;
        
            if (_shield <= 0 || _timeTillEnd >= TimeActive)
            {
                objectShield.SetActive(false);
                _player.Animator.SetBool(Earth, false);
                _player.Speed = _speed;
                timeTillCooldown = 0f;
                _timeTillEnd = 0f;
            }

            if (timeTillCooldown < Cooldown) timeTillCooldown += Time.deltaTime;
        
        }
    }
}
