using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FirePower : MonoBehaviour
{
    public float timeSinceShoot = 0f;
    public const float MaxBeforeAttack = 1f;
    private InputAction _attackAction;
    [SerializeField] private GameObject bullet;

    private Player _player;
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
        var transform1 = transform;
        var newGameObject = Instantiate(bullet, transform1.position + transform1.forward, bullet.transform.rotation);
        newGameObject.SetActive(true);
        timeSinceShoot = 0f;
    }
}
