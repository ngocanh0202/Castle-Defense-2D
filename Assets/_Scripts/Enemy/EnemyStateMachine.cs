using System.Collections;
using Common2D.CreateGameObject2D;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Gun enemyGun;
    [Header("Enemy States")]
    public EnemyBaseState CurrentState;
    public EnemyIdleState IdleState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyDeathState DeathState;
    public EnemyDamgedState DamagedState;
    public EnemyMainMissionState MainMissionState;
    [Header("Enemy speed")]
    [SerializeField] private float idleSpeed = 1f;
    [SerializeField] private float chaseSpeed = 2f;
    [Header("Enemy range")]
    [SerializeField] private float moveRange = 1f;
    [SerializeField] private float rangeToChase = 6f;
    [SerializeField] private float rangeToAttack = 4f;
    [Header("Other components")]
    public Transform mainTowerTransform;

    public void InitializeComponents()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (enemyGun == null)
        {
            enemyGun = GetComponentInChildren<Gun>();
        }
        
        if (mainTowerTransform == null)
        {
            mainTowerTransform = GameObject.Find("MainTower")?.transform;
        }
    }

    void Awake()
    {
        InitializeComponents();
        IdleState = new EnemyIdleState(this, animator, moveRange, idleSpeed, rangeToChase);
        ChaseState = new EnemyChaseState(this, animator, rangeToChase, chaseSpeed, rangeToAttack);
        AttackState = new EnemyAttackState(this, animator, rangeToAttack, enemyGun);
        DeathState = new EnemyDeathState(this, animator);
        DamagedState = new EnemyDamgedState(this, animator);
        if (mainTowerTransform != null)
        {
            MainMissionState = new EnemyMainMissionState(this, mainTowerTransform, animator, rangeToChase, idleSpeed);
            CurrentState = MainMissionState;
        }
        else
        {
            CurrentState = IdleState;
        }

        CurrentState.EnterState();
    }

    void Update()
    {
        if (CurrentState == null) return;
        CurrentState.UpdateState();
    }
    void FixedUpdate()
    {
        if (CurrentState == null) return;
        CurrentState.FixUpdateState();
    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (newState == null || newState == CurrentState) return;
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public bool IsState(EnemyBaseState other)
    {
        if (other == null) return false;
        return this.CurrentState == other;
    }

    void OnDrawGizmos()
    {
        if (CurrentState != null)
        {
            CurrentState.OnDrawGizmos();
        }
    }

    void OnEnable()
    {
        InitializeComponents();
        if (mainTowerTransform == null)
        {
            SwitchState(IdleState);
        }
        else
        {
            SwitchState(MainMissionState);
        }
    }
}
