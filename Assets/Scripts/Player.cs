using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Rigidbody _rigidbody;
    private Animator _animator;
    
    [SerializeField] private float jumpStrength = 20;
    [SerializeField] private float speed = 20;
    [SerializeField] private float rotationSpeed = 360;
    public const int MaxHealth = 100;
    public float health;
    
    private bool _haveAir;
    
    private bool _haveFire;

    private bool _haveEarth;

    private bool _haveWater;

    private AirPowerTODO _airPower;
    
    private FirePower _firePower;

    [SerializeField] private GameObject necklace;

    private EarthPower _earthPower;

    private WaterPowerTODO _waterPower;
    
    private Vector3 _gravityEffect; // Stocke l'effet de la gravité
    private bool _isGrounded;
    
    private static readonly int Forward = Animator.StringToHash("IsMovingForward");
    private static readonly int Backward = Animator.StringToHash("IsMovingBackward");
    
    private static readonly int Left = Animator.StringToHash("IsGoingLeft");
    private static readonly int Right = Animator.StringToHash("IsGoingRight");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int IsStanding = Animator.StringToHash("isStanding");
    private static readonly int Death1 = Animator.StringToHash("Death");
    private static readonly int FallSpeed = Animator.StringToHash("FallSpeed");

    // Start is called before the first frame update
    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        
        _airPower = GetComponent<AirPowerTODO>();
        _airPower.enabled = false;
        
        _earthPower = GetComponent<EarthPower>();
        _earthPower.enabled = false;
        
        _firePower = GetComponent<FirePower>();
        _firePower.enabled = false;
        necklace.SetActive(false);
        
        _waterPower = GetComponent<WaterPowerTODO>();
        _waterPower.enabled = false;
        
        health = MaxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.G)) _haveEarth = true;
        
        if (_haveAir && !_airPower.enabled) _airPower.enabled = true;

        if (_haveFire && !_firePower.enabled)
        {
            necklace.SetActive(true);
            _firePower.enabled = true;
        }
        
        if (_haveEarth && !_earthPower.enabled) _earthPower.enabled = true;

        if (_haveWater && !_waterPower.enabled) _waterPower.enabled = true;
        
        if (health <= 0) StartCoroutine(Death());
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 5f);
        _animator.SetBool(IsStanding, _isGrounded);
        
        // Déplacement avant/arrière
        var moveInput = _moveAction.ReadValue<Vector2>();
        var moveDirection = transform.forward * moveInput.y;
        
        _animator.SetBool(Forward, moveInput.y > 0.1);
        _animator.SetBool(Backward, moveInput.y < -0.1);
        
        // Mise à jour de la vélocité
        var horizontalVelocity = new Vector3(moveDirection.x * speed, _rigidbody.velocity.y, moveDirection.z * speed);
        _rigidbody.velocity = horizontalVelocity;
        
        _animator.SetFloat(FallSpeed, horizontalVelocity.y);
        
        // Rotation de la caméra avec A/D (gauche/droite)
        transform.Rotate(0, moveInput.x * rotationSpeed * Time.fixedDeltaTime, 0);
        _animator.SetBool(Left, moveInput.x > 0.1);
        _animator.SetBool(Right, moveInput.x < -0.1);

        // Saut
        if (!_isGrounded || !(_jumpAction.ReadValue<float>() > 0.5f)) return;
        _animator.SetTrigger(Jump);
        var velocity = _rigidbody.velocity;
        velocity.y = jumpStrength;
        _rigidbody.velocity = velocity;
    }
    
    private IEnumerator Death()
    {
        _animator.SetTrigger(Death1);
        yield return new WaitForSeconds(2f);
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
        if (other.gameObject.TryGetComponent<DeathPlane>(out _)) StartCoroutine(Death());
    }
}
