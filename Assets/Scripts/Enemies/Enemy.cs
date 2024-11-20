using UnityEngine;
using Pathfinding;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Fleeing,
        Hostile,
        Attacking,
    }

    private EnemyState currentState;
    public Transform _player;
    private Transform _transform;
    public float hostileSpeed = 5f;
    public float idleSpeed = 1f;
    private float currentSpeed;
    public float waypointAccuracy = 1f;
    private Path path;
    private int currentWaypoint;
    public float idleMoveRange = 10f;
    bool reachedEndOfPath = false;
    private Seeker _seeker;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool hostileTriggerStatus;
    private bool attackTriggerStatus;
    public LayerMask RaycastingMask;
    private float lastHorizontal;
    


    public float maxHP;
    public float enemyDefense;
    private float currentHP;
    public float attackDistance = 10f;

    private void Awake()
    {
        BecomeIdle();
        currentHP = maxHP;
        _transform = transform.parent.transform;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _seeker = GetComponent<Seeker>();
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _animator = transform.parent.GetComponentInChildren<Animator>();

        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        switch (currentState) // Run the current state's Update function
        {
            case EnemyState.Idle:
                IdleUpdate();
            break;

            case EnemyState.Fleeing:
                FleeingUpdate();
            break;

            case EnemyState.Hostile:
                HostileUpdate();
            break;

            case EnemyState.Attacking:
                AttackingUpdate();
            break;
        }   
    }

    private void FixedUpdate()
    {
        if (path != null)
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
            }
            else
            {
                reachedEndOfPath = false;
            }

            if (path.vectorPath[currentWaypoint] != null)
            {
                Vector2 directionToMove = ((Vector2)path.vectorPath[currentWaypoint] - _rigidbody.position).normalized;  
                Vector2 force = directionToMove * currentSpeed;
                

                _rigidbody.AddForce(force);
                float distance = Vector2.Distance(_rigidbody.position, path.vectorPath[currentWaypoint]);
                
                if (distance < waypointAccuracy)
                {
                    currentWaypoint++;
                }
            }
        }
        lastHorizontal = _rigidbody.linearVelocity.x;
        _animator.SetFloat("HorizontalDirection", lastHorizontal);
        if(_rigidbody.linearVelocity.x == 0)
        {
            _animator.SetFloat("LastHorizontal", lastHorizontal);
        }

        switch (currentState) // Run the current state's Update function
        {
            case EnemyState.Idle:
                IdleFixedUpdate();
            break;

            case EnemyState.Fleeing:
                FleeingFixedUpdate();
            break;

            case EnemyState.Hostile:
                HostileFixedUpdate();
            break;

            case EnemyState.Attacking:
                AttackingFixedUpdate();
            break;
        }   
    }

    
   
    #region IDLE STATE
    public void BecomeIdle()
    {
        currentState = EnemyState.Idle;
        currentSpeed = idleSpeed;
    }
    private void IdleUpdate()
    {
        if (hostileTriggerStatus == true) // If player is in range to start chasing
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, _player.position - transform.position, 100f, RaycastingMask);
            if (ray.collider != null)
            {
                if (ray.collider.gameObject.CompareTag("Player"))
                {
                    IdleExitLogic();
                    BecomeHostile();
                }
            }
        }
    }
    private void IdleFixedUpdate()
    {
        
    }
    private void IdlePath()
    {
        if (reachedEndOfPath || path == null)
        {
            Vector2 randomPoint = _rigidbody.position + Random.insideUnitCircle * idleMoveRange;
            _seeker.StartPath(_rigidbody.position, randomPoint, OnPathLoaded);
        }

    }
    private void IdleExitLogic()
    {

    }
    #endregion
    
    #region FLEEING STATE
    public void BecomeFleeing()
    {
        currentState = EnemyState.Fleeing;
    }
    private void FleeingUpdate()
    {
        
    }
    private void FleeingFixedUpdate()
    {
        
    }
    private void FleeingPath()
    {

    }
    #endregion
    
    #region HOSTILE STATE
    public void BecomeHostile()
    {
        currentState = EnemyState.Hostile;
        currentSpeed = hostileSpeed;
    }
    private void HostileUpdate()
    {
        if (hostileTriggerStatus == false)
        {
            HostileExitLogic();
            BecomeIdle();
        }
        if (attackTriggerStatus == true)
        {
            HostileExitLogic();
            BecomeAttacking();
        }
    }
    private void HostileFixedUpdate()
    {
        
    }
    private void HostilePath()
    {
        _seeker.StartPath(_rigidbody.position, _player.position, OnPathLoaded);
    }
    private void HostileExitLogic()
    {

    }
    #endregion
    
    #region ATTACKING STATE
    public void BecomeAttacking()
    {
        currentState = EnemyState.Attacking;
        Debug.Log("KILL !!!");
        StartCoroutine(RepeatAttack());
    }
    private void AttackingUpdate()
    {
        if (attackTriggerStatus == false)
        {
            AttackingExitLogic();
            BecomeHostile();
        }
    }
    private void AttackingFixedUpdate()
    {
        
    }
    private void AttackingPath()
    {
        _seeker.CancelCurrentPathRequest();
        path = null;
    }
    private void AttackingExitLogic()
    {

    }

    private IEnumerator RepeatAttack()
    {
        while (currentState == EnemyState.Attacking)
        {
            CastAttack();
            yield return new WaitForSeconds(1.5f);
        }
        yield return null;
    }

    private void CastAttack()
    {
        Debug.Log("wooooo woooo i shoot  energy ball at you  wooo i am scary");
    }
    #endregion


    #region Range Triggers
    public void HostileRadiusEntryTrigger()
    {
        hostileTriggerStatus = true;
    }
    public void HostileRadiusExitTrigger()
    {
        hostileTriggerStatus = false;
    }
    public void AttackRadiusEntryTrigger()
    {
        attackTriggerStatus = true;
    }
    public void AttackRadiusExitTrigger()
    {
        attackTriggerStatus = false;
    }
    #endregion

    #region Health/Die Functions
    public void ReceiveHarm(HarmfulObjectScript harmSource)
    {
        if (harmSource.canDamageEnemy)
        {
            Damage(harmSource.damageAmount * enemyDefense);
            if (harmSource.destroyOnContact)
            {
                harmSource.DestroySelf();
            }
        }
        if (currentState==EnemyState.Idle && harmSource.Source.CompareTag("Player"))
        {
            
        }
    }
    public void Damage(float damageAmount)
    {
        currentHP -= damageAmount;

        if (damageAmount > 0)
        {
            _animator.Play("Hurt", -1, 0f);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(_transform.gameObject);
    }
    #endregion


    #region PATHFINDING
    private IEnumerator UpdatePath()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            if (_seeker.IsDone())
            {
                switch (currentState) // Run the current state's Path function
                {
                    case EnemyState.Idle:
                        IdlePath();
                    break;

                    case EnemyState.Fleeing:
                        FleeingPath();
                    break;

                    case EnemyState.Hostile:
                        HostilePath();
                    break;

                    case EnemyState.Attacking:
                        AttackingPath();
                    break;
                }
            }
        }
    }

    private void OnPathLoaded(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    #endregion
}
