using System;
using UnityEngine;
using Common2D;

public class TowerTargetingSystem : Singleton<TowerTargetingSystem>
{
    [Header("Targeting Settings")]
    [SerializeField] private LineRenderer targetingLine;
    [SerializeField] private Color defaultLineColor = Color.yellow;
    [SerializeField] private Color validTargetLineColor = Color.red;
    [SerializeField] private float lineWidth = 0.1f;

    [Header("Targeting State")]
    [SerializeField] private TowerDefenseBehaviour targetingTower;
    [SerializeField] private Transform priorityTarget;
    [SerializeField] private bool isDragging;
    [SerializeField] private Vector2 lastWorldPosition;

    protected override void Awake()
    {
        base.Awake();
        SetupTargetingLine();
    }

    void Start()
    {
        InputManager.Instance.OnTowerDrag += HandleTowerDrag;
    }

    private void SetupTargetingLine()
    {
        if (targetingLine == null)
        {
            GameObject lineObj = new GameObject("TargetingLine");
            lineObj.transform.SetParent(transform);
            targetingLine = lineObj.AddComponent<LineRenderer>();
            targetingLine.startWidth = lineWidth;
            targetingLine.endWidth = lineWidth;
            targetingLine.material = new Material(Shader.Find("Sprites/Default"));
            targetingLine.startColor = defaultLineColor;
            targetingLine.endColor = defaultLineColor;
            targetingLine.positionCount = 2;
            targetingLine.enabled = false;
        }
    }

    private void HandleTowerDrag(Vector2 startWorldPos, Vector2 currentWorldPos)
    {
        isDragging = true;
        lastWorldPosition = currentWorldPos;

        if (targetingTower == null)
        {
            TowerInteraction tower = GetTowerAtPosition(startWorldPos);
            if (tower != null)
            {
                targetingTower = tower.TowerBehaviour;
            }
        }

        if (targetingTower != null)
        {
            targetingLine.enabled = true;
            targetingLine.SetPosition(0, targetingTower.transform.position);

            Transform potentialTarget = GetEnemyAtPosition(currentWorldPos);
            if (potentialTarget != null)
            {
                targetingLine.SetPosition(1, potentialTarget.position);
                targetingLine.startColor = validTargetLineColor;
                targetingLine.endColor = validTargetLineColor;
            }
            else
            {
                targetingLine.SetPosition(1, (Vector3)currentWorldPos);
                targetingLine.startColor = defaultLineColor;
                targetingLine.endColor = defaultLineColor;
            }
        }
    }

    void Update()
    {
        if (isDragging && Input.touchCount == 0)
        {
            FinishTargeting();
        }
    }

    private void FinishTargeting()
    {
        if (targetingLine.enabled)
        {
            Transform target = GetEnemyAtPosition(lastWorldPosition);
            if (target != null)
            {
                SetPriorityTarget(targetingTower, target);
            }
            else
            {
                ClearPriorityTarget(targetingTower);
            }
        }

        targetingLine.enabled = false;
        isDragging = false;
        targetingTower = null;
    }

    private TowerInteraction GetTowerAtPosition(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.GetComponent<TowerInteraction>();
        }
        return null;
    }

    private Transform GetEnemyAtPosition(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider != null)
        {
            IReceiveDamage enemy = hit.collider.GetComponent<IReceiveDamage>();
            if (enemy != null)
            {
                return hit.collider.transform;
            }
        }
        return null;
    }

    public void SetPriorityTarget(TowerDefenseBehaviour tower, Transform target)
    {
        if (tower == null) return;
        
        priorityTarget = target;
        
        EnemyPriorityTarget priorityComponent = target.gameObject.AddComponent<EnemyPriorityTarget>();
        priorityComponent.OnDestroyed += () => HandlePriorityTargetDestroyed(tower);
        
        Debug.Log($"Priority target set for tower {tower.name}: {target.name}");
    }

    public void ClearPriorityTarget(TowerDefenseBehaviour tower)
    {
        if (tower == null) return;
        
        priorityTarget = null;
        Debug.Log($"Priority target cleared for tower {tower.name}");
    }

    private void HandlePriorityTargetDestroyed(TowerDefenseBehaviour tower)
    {
        if (tower != null)
        {
            ClearPriorityTarget(tower);
        }
    }

    public Transform GetPriorityTarget(TowerDefenseBehaviour tower)
    {
        return priorityTarget;
    }

    public bool HasPriorityTarget(TowerDefenseBehaviour tower)
    {
        return priorityTarget != null && tower != null;
    }
}

public class EnemyPriorityTarget : MonoBehaviour
{
    public event Action OnDestroyed;

    void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
