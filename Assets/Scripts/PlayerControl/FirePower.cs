using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class FirePower : MonoBehaviour
    {
        public float timeSinceShoot;
        public const float MaxBeforeAttack = 1f;
        private InputAction _attackAction;
        private Player _player;
    
        [SerializeField] private GameObject bullet;
    
        private static readonly int Fire = Animator.StringToHash("Fire");

        // Start is called before the first frame update
        private void Start()
        {
            _attackAction = InputSystem.actions.FindAction("Attack");
            _player = GetComponent<Player>();
        }

        // Update is called once per frame
        private void Update()
        {
            var attackValue = _attackAction.ReadValue<float>();
            if ( Math.Abs(attackValue - 1) < 0.001f && timeSinceShoot >= MaxBeforeAttack )
            {
                Shoot();
            }

            if (_player.health < Player.MaxHealth)
            {
                _player.health += 0.05f * Time.deltaTime;
            }
            else
            {
                _player.health = Player.MaxHealth;
            }
            timeSinceShoot += Time.deltaTime;

        }
    
        private void Shoot()
        {
            _player.Animator.SetTrigger(Fire);
            var transform1 = transform;
            var newGameObject = Instantiate(bullet, transform1.position + transform1.forward,transform1.rotation);
            newGameObject.SetActive(true);
            timeSinceShoot = 0f;
        }
    }
}
