using System;
using UiMenu;
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
        
        [SerializeField] private GameObject shield;
        
        [SerializeField] private float jumpStrength = 20;
    
        [SerializeField] private float rotationSpeed = 360;
        public const int MaxHealth = 100;
        public float health;
    
        private bool _haveAir;
    
        private bool _haveFire;

        private bool _haveEarth;

        private bool _haveWater;

        private AirPowerTodo _airPower;
    
        [SerializeField] private GameObject bracelet;
        
        private FirePower _firePower;

        [SerializeField] private GameObject necklace;

        private EarthPower _earthPower;
        
        [SerializeField] private GameObject gloves;

        private WaterPowerTODO _waterPower;
        
        [SerializeField] private GameObject shoes;
    
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
            bracelet.SetActive(false);
            
            _earthPower = GetComponent<EarthPower>();
            _earthPower.enabled = false;
            gloves.SetActive(false);
        
            _firePower = GetComponent<FirePower>();
            _firePower.enabled = false;
            necklace.SetActive(false);
        
            _waterPower = GetComponent<WaterPowerTODO>();
            _waterPower.enabled = false;
            shoes.SetActive(false);
        
            health = MaxHealth;
            _camera = Camera.current;
            Speed = 10f;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.G)) _haveFire = true;
            if (Input.GetKey(KeyCode.F)) _haveEarth = true;

            if (_haveAir && !_airPower.enabled)
            {
                bracelet.SetActive(true);
                _airPower.enabled = true;
            }

            if (_haveFire && !_firePower.enabled)
            {
                necklace.SetActive(true);
                _firePower.enabled = true;
            }

            if (_haveEarth && !_earthPower.enabled)
            {
                gloves.SetActive(true);
                _earthPower.enabled = true;
            }

            if (_haveWater && !_waterPower.enabled)
            {
                shoes.SetActive(true);
                _waterPower.enabled = true;
            }
        
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
            StartCoroutine(Game.OnDeath());
        }

        public void OnHit(float damage)
        {
            if (shield.activeSelf) return;
            if (!IsAnimationPlaying("GetHit"))
            {
                Animator.SetTrigger(Hit);
            }
           
            if (_haveEarth && _earthPower.passiveShield > 0f )
            {
                _earthPower.passiveShield -= damage;
                if (!(_earthPower.passiveShield < 0f)) return;
                var supplement = _earthPower.passiveShield;
                _earthPower.passiveShield = 0f;
                health += supplement;
            }
            else
            {
                health -= damage;
            }
        }
    
        private bool IsAnimationPlaying(string animationName)
        {
            var currentState = Animator.GetCurrentAnimatorStateInfo(1); // 0 correspond à la première couche
            return currentState.IsName(animationName) && currentState.normalizedTime < 1f;
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.TryGetComponent<AirBracelet>(out _) && _interact.ReadValue<float>()>0.5f )
            {
                _haveAir = true;
                other.gameObject.SetActive(false);
            }

            if (other.gameObject.TryGetComponent<FireNecklace>(out _) && _interact.ReadValue<float>()>0.5f)
            {
                _haveFire = true;
                other.gameObject.SetActive(false);
            }
            
            if (other.gameObject.TryGetComponent<EarthGloves>(out _) && _interact.ReadValue<float>()>0.5f)
            {
                _haveEarth = true;
                other.gameObject.SetActive(false);
            }
            
            if (other.gameObject.TryGetComponent<WaterShoes>(out _) && _interact.ReadValue<float>()>0.5f )
            {
                _haveWater = true;
                other.gameObject.SetActive(false);
            }
            if (other.gameObject.TryGetComponent<DeathPlane>(out _)) Death();
        }
    }
}
