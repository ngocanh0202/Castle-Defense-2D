using System.Collections;
using Common2D.CreateGameObject2D;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyStat))]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public EnemyStat enemyStat;
    public Gun enemyGun;
    [Header("Enemy States")]
    public EnemyBaseState CurrentState;
    public EnemyIdleState IdleState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyMainMissionState MainMissionState;
    [Header("Other components")]
    public Transform mainTowerTransform;
    [SerializeField] private string CurrentStateName { get => CurrentState?.GetType().Name; }

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
        if (enemyStat == null)
        {
            enemyStat = GetComponent<EnemyStat>();
        }
    }

    void Awake()
    {
        InitializeComponents();
        InitializeStates();
        CurrentState.EnterState();
    }

    private void InitializeStates()
    {
        IdleState = new EnemyIdleState(this, animator, enemyStat);
        ChaseState = new EnemyChaseState(this, animator, enemyStat);
        AttackState = new EnemyAttackState(this, animator, enemyStat, enemyGun);
        if (mainTowerTransform == null)
        {
            mainTowerTransform = GameObject.Find("MainTower")?.transform;
        }
        if (mainTowerTransform != null)
        {
            MainMissionState = new EnemyMainMissionState(this, mainTowerTransform, animator, enemyStat);
            CurrentState = MainMissionState;
        }
        else
        {
            CurrentState = IdleState;
        }
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
        InitializeStates();
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
