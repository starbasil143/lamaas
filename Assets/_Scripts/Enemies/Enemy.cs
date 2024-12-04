using UnityEngine;
using Pathfinding;
using System.Collections;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Fleeing,
        Hostile,
        Attacking,
    }

    public enum EnemyType
    {
        WeakCultist,
        C_Bunny,
        C_Deer,
        C_Eel,
        C_Bear,
    }

    public enum IdleBehavior
    {
        None,
        Roam,
    }
    public enum HostileBehavior
    {
        None,
        Chase,
        CircleAndCharge,
    }
    public enum AttackBehavior
    {
        None,
        ShootSingleProjectile,
        LightCharge,
        HeavyCharge,
        CircleAndCharge,
    }
    public enum FleeBehavior
    {
        None,
        Retreat,
    }

    private EnemyState currentState;
    public EnemyType enemyType;
    public IdleBehavior idleBehavior;
    public HostileBehavior hostileBehavior;
    public AttackBehavior attackBehavior;
    public FleeBehavior fleeBehavior;
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
    private bool isFacingRight = true;
    public LayerMask RaycastingMask;
    private float lastHorizontal;
    public float idleTimeBetweenPaths = 1f;
    public float hostileTimeBetweenPaths = 1f;
    private float timeBetweenPaths;
    [SerializeField] private GameObject projectile;
    public GameObject EnemyGFX;
    public GameObject EnemyHarmBox;
    Coroutine repeatCastAttackCoroutine;
    Coroutine maybeStopAttacking;
    Coroutine lightChargeAttackCoroutine;
    public float circleAroundRange = 2f;



    public float maxHP;
    public float enemyDefense;
    private float currentHP;
    public float attackDistance = 10f;
    public float timeBetweenCastAttacks = 1.5f;
    public float timeBetweenLightChargeAttacks = 1.8f;
    public float timeBetweenCircleCharges = 2f;
    private float circleChargeTimer = 0f;
    public float lungeForceMultiplier = 20f;
    public float chargeTime = .5f;
    public GameObject dropOnDeath;
    public GameObject expPrefab;

    public UnityEvent ExecuteOnDeath;

    private void Awake()
    {
        _animator = EnemyGFX.GetComponent<Animator>();
        BecomeIdle();
        currentHP = maxHP;
        _transform = transform.parent.transform;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _seeker = GetComponent<Seeker>();
        _rigidbody = GetComponentInParent<Rigidbody2D>();

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

            if (!reachedEndOfPath)
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
        FaceCorrectDirection(_rigidbody.linearVelocity);

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
        timeBetweenPaths = idleTimeBetweenPaths;
            _animator.Play("Walk");
    }
    private void IdleUpdate()
    {
        if (hostileTriggerStatus == true) // If player is in range to start chasing
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, _player.position - transform.position, 100f, RaycastingMask);
            if (ray.collider != null)
            {   
                Debug.Log(ray.collider.gameObject);
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
        switch (idleBehavior)
        {
            case IdleBehavior.Roam:
                if (reachedEndOfPath || path == null)
                {
                    Vector2 randomPoint = _rigidbody.position + Random.insideUnitCircle * idleMoveRange;
                    _seeker.StartPath(_rigidbody.position, randomPoint, OnPathLoaded);
                }
            break;
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
        timeBetweenPaths = hostileTimeBetweenPaths;
        _animator.Play("Walk");
    }
    private void HostileUpdate()
    {
        switch (hostileBehavior)
        {
            case HostileBehavior.Chase:
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
                break;

            case HostileBehavior.CircleAndCharge:
                if (hostileTriggerStatus == false)
                {
                    HostileExitLogic();
                    BecomeIdle();
                }
                if (circleChargeTimer >= timeBetweenCircleCharges)
                {
                    circleChargeTimer = 0f;
                    HostileExitLogic();
                    BecomeAttacking();
                }
                circleChargeTimer += Time.deltaTime;
                break;


            
        }
    }
    private void HostileFixedUpdate()
    {
        
    }
    private void HostilePath()
    {
        switch (hostileBehavior)
        {
            case HostileBehavior.Chase:
                _seeker.StartPath(_rigidbody.position, _player.position, OnPathLoaded);
                break;

            case HostileBehavior.CircleAndCharge:
                _seeker.StartPath(_rigidbody.position, _player.position + circleAroundRange * ((Quaternion.Euler(0f, 0f, 90f)) * (_player.position - (Vector3)_rigidbody.position)), OnPathLoaded);
                break;
        }
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

        switch (attackBehavior)
        {
            case AttackBehavior.ShootSingleProjectile:
                repeatCastAttackCoroutine = StartCoroutine(RepeatCastAttack());
            break;

            case AttackBehavior.LightCharge:
                lightChargeAttackCoroutine = StartCoroutine(RepeatLightCharge());
            break;

            case AttackBehavior.CircleAndCharge:
                StartCoroutine(CircleChargeAttack());
            break;
        }
        
    }
    private void AttackingUpdate()
    {
        switch (attackBehavior)
        {
            case AttackBehavior.CircleAndCharge:
                break;

            default:
                if (attackTriggerStatus == false)
                {
                    if (maybeStopAttacking != null)
                    {
                        StopCoroutine(maybeStopAttacking);
                    }
                    StartCoroutine(MaybeStopAttackingAfterALittleBit(3f));
                }
                break;
        }
    }
    private void AttackingFixedUpdate()
    {
        
    }
    private void AttackingPath()
    {
        switch (attackBehavior)
        {
            default:
                _seeker.CancelCurrentPathRequest();
                path = null;
            break;
        }
    }
    private void AttackingExitLogic()
    {
        switch (attackBehavior)
        {
            case AttackBehavior.ShootSingleProjectile:
                StopCoroutine(repeatCastAttackCoroutine);
            break;

            case AttackBehavior.LightCharge:
                StopCoroutine(lightChargeAttackCoroutine);
            break;
        }
    }

    private IEnumerator RepeatCastAttack()
    {
        while (currentState == EnemyState.Attacking)
        {
            yield return new WaitForSeconds(timeBetweenCastAttacks);
            CastAttack();
        }
        yield return null;
    }

    private IEnumerator RepeatLightCharge()
    {
        while (currentState == EnemyState.Attacking)
        {
            yield return new WaitForSeconds(timeBetweenLightChargeAttacks * Random.Range(.8f, 1.2f));
            StartCoroutine(LungeAttack());
        }
        yield return null;
    }

    private IEnumerator CircleChargeAttack()
    {
        FaceCorrectDirection(_player.position - (Vector3)_rigidbody.position);
        _seeker.CancelCurrentPathRequest();
        path = null;
        _rigidbody.linearVelocity = Vector3.zero;
        _animator.Play("Charge");
        yield return new WaitForSeconds(chargeTime);
        _animator.Play("Run");
        EnemyHarmBox.SetActive(true);
        Vector2 lungeForce = ((Vector2)_player.position - (Vector2)transform.position).normalized * lungeForceMultiplier;
        _rigidbody.AddForce(lungeForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.3f);
        lungeForce = Vector3.RotateTowards(lungeForce, _player.position-_transform.position, .5f, 0f);
        _rigidbody.AddForce(lungeForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.3f);
        EnemyHarmBox.SetActive(false);
        AttackingExitLogic();
        BecomeHostile();
    }

    private IEnumerator LungeAttack()
    {
        
        _animator.Play("Walk");
        Vector2 lungeForce = ((Vector2)_player.position - (Vector2)transform.position).normalized * lungeForceMultiplier;
        _rigidbody.AddForce(lungeForce, ForceMode2D.Impulse);

        EnemyHarmBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        EnemyHarmBox.SetActive(false);
    }

    private void CastAttack()
    {
        GameObject Projectile = Instantiate(projectile, new Vector2(transform.position.x, transform.position.y+0.5f), Quaternion.Euler(0, 0, 0)); // Make the projectile
        Projectile.GetComponent<HarmfulObjectScript>().Source = gameObject;
        Physics2D.IgnoreCollision(Projectile.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>());
        Physics2D.IgnoreCollision(Projectile.GetComponent<Collider2D>(), EnemyGFX.GetComponent<Collider2D>());
        
        Vector2 shootForce = ((Vector2)_player.position - (Vector2)transform.position).normalized * 5; 
        Projectile.GetComponent<Rigidbody2D>().AddForce(shootForce, ForceMode2D.Impulse); // Give the projectile a force so it moves
        
    }

    private IEnumerator MaybeStopAttackingAfterALittleBit(float littleBit)
    {
        yield return new WaitForSeconds(littleBit);
        if (!attackTriggerStatus)
        {
            AttackingExitLogic();
            BecomeHostile();
        }
        yield return null;
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

    public void VinePull(HarmfulObjectScript harmSource)
    {
        if (harmSource.canDamageEnemy)
        {
            Damage(harmSource.damageAmount * enemyDefense);

            if (harmSource.Source != null)
            {
                Vector2 enemyPosition = _rigidbody.position;
                Vector2 playerPosition = harmSource.Source.transform.position;

                // Calculate midpoint between enemy and player
                Vector2 pullDestination = Vector2.Lerp(enemyPosition, playerPosition, 0.5f);

                // Move enemy to midpoint
                _rigidbody.MovePosition(pullDestination);
            }

            if (harmSource.destroyOnContact)
            {
                harmSource.DestroySelf();
            }
        }
    }

    public void Damage(float damageAmount)
    {
        currentHP -= damageAmount;

        if (damageAmount > 0)
        {
            _animator.Play("Hurt", -1, 0f);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDamage, transform.position); // change to enemy damage later (probably)
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (ExecuteOnDeath != null)
        {
            ExecuteOnDeath.Invoke();
        }
        if (dropOnDeath != null)
        {
            Instantiate(dropOnDeath, transform.position, Quaternion.identity);
        }
        if(expPrefab != null)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity);
        }
        Destroy(_transform.gameObject);
    }
    #endregion


    #region PATHFINDING
    private IEnumerator UpdatePath()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenPaths);
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
                if (path != null)
                {
                    //FaceCorrectDirection(((Vector2)path.vectorPath[0] - _rigidbody.position).normalized);
                }
            }
        }
    }

    private void FaceCorrectDirection(Vector2 velocity)
    {
        if (velocity.x < -0.1f && isFacingRight)
        {
            _transform.rotation = Quaternion.Euler(new Vector3(_transform.position.x, 180f, _transform.position.z));
            isFacingRight = false;
        }
        if (velocity.x > 0.1f && !isFacingRight)
        {
            _transform.rotation = Quaternion.Euler(new Vector3(_transform.position.x, 0f, _transform.position.z));
            isFacingRight = true;
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

    private void PauseEnemy()
    {

    }

    private void ResumeEnemy()
    {

    }

}
