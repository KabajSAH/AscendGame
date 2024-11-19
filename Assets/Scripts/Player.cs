using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Rigidbody _rigidbody;
    [SerializeField] private float jumpStrength = 20;
    [SerializeField] private float speed = 20;
    [SerializeField] private float rotationSpeed = 180;
    public const int MaxHealth = 100;
    public float health;
    
    private bool _haveAir;
    
    private bool _haveFire;

    private bool _haveEarth;

    private bool _haveWater;

    private AirPowerTODO _airPower;
    
    private FirePower _firePower;

    private EarthPower _earthPower;

    private WaterPowerTODO _waterPower;
    
    private Vector3 _gravityEffect; // Stocke l'effet de la gravité
    private bool _isGrounded; 
    
    // Start is called before the first frame update
    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        
        _rigidbody = GetComponent<Rigidbody>();
        _airPower = GetComponent<AirPowerTODO>();
        _airPower.enabled = false;
        
        _earthPower = GetComponent<EarthPower>();
        _earthPower.enabled = false;
        
        _firePower = GetComponent<FirePower>();
        _firePower.enabled = false;
        
        _waterPower = GetComponent<WaterPowerTODO>();
        _waterPower.enabled = false;
        
        health = 100f;
    }

    // Update is called once per frame
    private void Update()
    {
        // Détecter si le personnage est au sol
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        
        var moveInput = _moveAction.ReadValue<Vector2>(); // Par défaut, W/S ou flèches haut/bas
        var moveDirection = transform.forward * moveInput.y; // Déplace le joueur sur l'axe Z (avant/arrière)
        
        // Mise à jour de la vélocité horizontale sans toucher à la composante verticale
        var horizontalVelocity = new Vector3(moveDirection.x * speed, _rigidbody.velocity.y, moveDirection.z * speed);
        _rigidbody.velocity = horizontalVelocity; // Applique la vélocité horizontale sans affecter la composante verticale
        
        // Déplacement de la caméra avec A/D (gauche/droite)
        transform.Rotate(0, moveInput.x * rotationSpeed * Time.deltaTime, 0);
        
        // Saut
        if (_isGrounded && _jumpAction.ReadValue<float>() > 0.5f)
        {
            var velocity = _rigidbody.velocity;
            velocity.y = jumpStrength; // Définit une vitesse verticale constante
            _rigidbody.velocity = velocity;
        }

        if (Input.GetKey(KeyCode.G)) _haveEarth = true;
        
        if (_haveAir && !_airPower.enabled) _airPower.enabled = true;
        
        if (_haveFire && !_firePower.enabled) _firePower.enabled = true;
        
        if (_haveEarth && !_earthPower.enabled) _earthPower.enabled = true;

        if (_haveWater && !_waterPower.enabled) _waterPower.enabled = true;
        
        if (health <= 0) Death();
    }

    private static void Death()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<AirBracelet>(out _))
        {
            _haveAir = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.TryGetComponent<FireNecklace>(out _))
        {
            _haveFire = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.TryGetComponent<DeathPlane>(out _)) Death();
    }
}
