using System;
using System.Collections;
using PlayerControl;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private const int MaxHealth = 30;
    [SerializeField] private float health;
    
    [Header("Zone de déplacement")]
    [SerializeField] private Vector3 centerPoint; // Le centre de la sphère
    [SerializeField] private float patrolRadius = 10f; // Rayon de la zone de patrouille
    [SerializeField] private float detectionRadius = 5f; // Rayon de détection du joueur

    [Header("Comportement")]
    [SerializeField] private float attackDistance = 2f; // Distance pour attaquer le joueur
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float idleDuration = 1f;

    private Transform _player;
    private NavMeshAgent _agent;
    private Vector3 _targetPosition;
    private float _attackTimer;
    private float _idleTimer;
    private Vector3 _lastKnownPlayerPosition;
    private bool _isDying = false;

    private enum EnemyState { Patrolling, Chasing, Attacking, Idling }
    private EnemyState _currentState = EnemyState.Idling;
    private static readonly int IsMovable = Animator.StringToHash("IsMovable");
    private static readonly int Hited = Animator.StringToHash("Hited");
    private static readonly int Dying = Animator.StringToHash("Dying");

    private void Start()
    {
        health = MaxHealth;
        _player = GameObject.FindGameObjectWithTag("Player").transform; // Assurez-vous que le joueur a le tag "Player"
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = patrolSpeed;
        _animator = GetComponentInChildren<Animator>();
        
        SetNewPatrolPoint(); // Définit un premier point de patrouille
    }

    private void Update()
    {
        if (_isDying) return;
        
        _attackTimer += Time.deltaTime;

        var distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        switch (_currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (IsPlayerVisible())
                {
                    _currentState = EnemyState.Chasing;
                    _agent.speed = chaseSpeed;
                }
                else if (Vector3.Distance(transform.position, _targetPosition) < 1f)
                {
                    TransitionToState(EnemyState.Idling);
                }
                break;

            case EnemyState.Chasing:
                ChasePlayer();
                if (!IsPlayerVisible())
                {
                    TransitionToState(EnemyState.Idling);
                }
                else if (distanceToPlayer <= attackDistance)
                {
                    TransitionToState(EnemyState.Attacking);
                }
                break;

            case EnemyState.Attacking:
                AttackPlayer();
                if (distanceToPlayer > attackDistance)
                {
                    TransitionToState(EnemyState.Idling);
                }
                break;
            case EnemyState.Idling:
                Idle();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Patrol()
    {
        if (!_agent.pathPending && Vector3.Distance(transform.position, _targetPosition) < 0.5f)
        {
            SetNewPatrolPoint();
        }
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        if (!(_attackTimer >= attackCooldown)) return;
        if (_player.TryGetComponent<Player>(out var pComponent))
        {
            _animator.SetTrigger(Random.value < 0.5f ? "Attacking1" : "Attacking2");
            pComponent.OnHit();
        }
        _attackTimer = 0f;
        TransitionToState(EnemyState.Idling);
        // Ajoutez ici le code pour infliger des dégâts au joueur
    }

    private void Idle()
    {
        _agent.isStopped = true;
        _idleTimer += Time.deltaTime;

        if (!(_idleTimer >= idleDuration)) return;
        _idleTimer = 0f;

        if (_currentState == EnemyState.Idling && !IsPlayerVisible())
        {
            SetNewPatrolPoint();
            TransitionToState(EnemyState.Patrolling);
        }
        else
        {
            TransitionToState(EnemyState.Patrolling);
        }
    }
    
    private void SetNewPatrolPoint()
    {
        var randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += centerPoint;

        if (!NavMesh.SamplePosition(randomDirection, out var navHit, patrolRadius, NavMesh.AllAreas)) return;
        _targetPosition = navHit.position;
        _agent.SetDestination(_targetPosition);
    }

    private void TransitionToState(EnemyState newState)
    {
        _currentState = newState;

        switch (newState)
        {
            case EnemyState.Patrolling:
                _agent.isStopped = false;
                _animator.SetBool(IsMovable, true);
                _agent.speed = patrolSpeed;
                break;

            case EnemyState.Chasing:
                _agent.isStopped = false;
                _animator.SetBool(IsMovable, true);
                _agent.speed = chaseSpeed;
                break;

            case EnemyState.Attacking:
                _agent.isStopped = true;
                _animator.SetBool(IsMovable, false);
                break;

            case EnemyState.Idling:
                _agent.isStopped = true;
                _animator.SetBool(IsMovable, false);
                _idleTimer = 0f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    
    private bool IsPlayerVisible()
    {
        return Vector3.Distance(transform.position, _player.position) <= detectionRadius;
    }

    public void GetHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _animator.SetTrigger(Dying);
            _isDying = true;
            _agent.isStopped = true;
            StartCoroutine(HandleDeath());

        }
        else _animator.SetTrigger(Hited);
    }

    private IEnumerator HandleDeath()
    {
        // Attendre la fin de la frame pour garantir que l'animation est appliquée
        yield return null;

        // Récupérer la durée de l'animation
        var animationLength = _animator.GetCurrentAnimatorStateInfo(2).length;

        // Détruire l'objet après la durée de l'animation
        Destroy(gameObject, animationLength);
    }
    
    private void OnDrawGizmosSelected()
    {
        // Dessiner les rayons de la zone de patrouille et de détection
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPoint, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    
}
