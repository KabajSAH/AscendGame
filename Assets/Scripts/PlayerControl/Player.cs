using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UselessScripts;

namespace PlayerControl
{
    public class Player : MonoBehaviour
    {
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _interact;
        private Rigidbody _rigidbody;
        private Camera _camera;
    
        [SerializeField] private float jumpStrength = 20;
    
        [SerializeField] private float rotationSpeed = 360;
        public const int MaxHealth = 100;
        public float health;
    
        private bool _haveAir;
    
        private bool _haveFire;

        private bool _haveEarth;

        private bool _haveWater;

        private AirPowerTodo _airPower;
    
        private FirePower _firePower;

        [SerializeField] private GameObject necklace;

        private EarthPower _earthPower;

        private WaterPowerTODO _waterPower;
    
        private bool _isGrounded;
    
        private static readonly int Forward = Animator.StringToHash("IsMovingForward");
        private static readonly int Backward = Animator.StringToHash("IsMovingBackward");
    
        private static readonly int Left = Animator.StringToHash("IsGoingLeft");
        private static readonly int Right = Animator.StringToHash("IsGoingRight");
        private static readonly int IsStanding = Animator.StringToHash("isStanding");
        private static readonly int Death1 = Animator.StringToHash("Death");
        private static readonly int Hit = Animator.StringToHash("Hit");
    
        public Animator Animator { get; private set; }
    
        public float Speed { get; set; }
        // Start is called before the first frame update
        private void Start()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _interact = InputSystem.actions.FindAction("Interact");
        
            _rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
        
            _airPower = GetComponent<AirPowerTodo>();
            _airPower.enabled = false;
        
            _earthPower = GetComponent<EarthPower>();
            _earthPower.enabled = false;
        
            _firePower = GetComponent<FirePower>();
            _firePower.enabled = false;
            necklace.SetActive(false);
        
            _waterPower = GetComponent<WaterPowerTODO>();
            _waterPower.enabled = false;
        
            health = MaxHealth;
            _camera = Camera.current;
            Speed = 10f;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.G)) _haveEarth = true;
            if (Input.GetKey(KeyCode.F)) _haveFire = true;
        
            if (_haveAir && !_airPower.enabled) _airPower.enabled = true;

            if (_haveFire && !_firePower.enabled)
            {
                necklace.SetActive(true);
                _firePower.enabled = true;
            }
        
            if (_haveEarth && !_earthPower.enabled) _earthPower.enabled = true;

            if (_haveWater && !_waterPower.enabled) _waterPower.enabled = true;
        
            if (health <= 0) Death();
        }

        private void FixedUpdate()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
            Animator.SetBool(IsStanding, _isGrounded);
        
            // Déplacement avant/arrière
            var moveInput = _moveAction.ReadValue<Vector2>();
            var moveDirection = transform.forward * moveInput.y;
            var targetPosition = transform.position + moveDirection * (Speed * Time.fixedDeltaTime);
        
            Animator.SetBool(Forward, moveInput.y > 0.1);
            Animator.SetBool(Backward, moveInput.y < -0.1);
        
            _rigidbody.MovePosition(targetPosition);
        
            // Rotation de la caméra avec A/D (gauche/droite)
            transform.Rotate(0, moveInput.x * rotationSpeed * Time.fixedDeltaTime, 0);
            Animator.SetBool(Left, moveInput.x > 0.1);
            Animator.SetBool(Right, moveInput.x < -0.1);

            // Saut
            if (!_isGrounded || !(_jumpAction.ReadValue<float>() > 0.5f)) return;
            var velocity = _rigidbody.velocity;
            velocity.y = jumpStrength;
            _rigidbody.velocity = velocity;
        }
    
        private void Death()
        {
            Animator.SetTrigger(Death1);
            StartCoroutine(global::Death.OnDeath());
        }

    

        public void OnHit()
        {
            Animator.SetTrigger(Hit);
            if (_haveEarth && _earthPower.passiveShield > 0f )
            {
                _earthPower.passiveShield -= 10f;
                if (!(_earthPower.passiveShield < 0f)) return;
                var supplement = _earthPower.passiveShield;
                _earthPower.passiveShield = 0f;
                health += supplement;
            }
            else
            {
                health -= 10f;
            }
        }
    
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent<AirBracelet>(out _) )
            {
                _haveAir = true;
                Destroy(other.gameObject);
            }

            if (other.gameObject.TryGetComponent<FireNecklace>(out _) && _interact.ReadValue<float>()>0.5f)
            {
                _haveFire = true;
                Destroy(other.gameObject);
            }
            if (other.gameObject.TryGetComponent<DeathPlane>(out _)) Death();
        }
    }
}
