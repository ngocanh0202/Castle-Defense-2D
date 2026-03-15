using System;
using System.Collections.Generic;
using UnityEngine;
using Common2D;

public enum FormationType
{
    Grid,
    Line,
    VShape
}

public class UnitFormationSystem : Singleton<UnitFormationSystem>
{
    [Header("Formation Settings")]
    [SerializeField] private FormationType defaultFormation = FormationType.Grid;
    [SerializeField] private float unitSpacing = 1.5f;
    [SerializeField] private float formationMoveSpeed = 3f;

    [Header("Active Formations")]
    private Dictionary<int, List<Transform>> activeFormations = new Dictionary<int, List<Transform>>();

    protected override void Awake()
    {
        base.Awake();
    }

    public Vector3[] CalculateFormationPositions(Vector3 spawnPointPosition, int unitCount, FormationType formationType)
    {
        Vector3[] positions = new Vector3[unitCount];

        switch (formationType)
        {
            case FormationType.Grid:
                positions = CalculateGridFormation(spawnPointPosition, unitCount);
                break;
            case FormationType.Line:
                positions = CalculateLineFormation(spawnPointPosition, unitCount);
                break;
            case FormationType.VShape:
                positions = CalculateVShapeFormation(spawnPointPosition, unitCount);
                break;
        }

        return positions;
    }

    private Vector3[] CalculateGridFormation(Vector3 spawnPoint, int unitCount)
    {
        int columns = Mathf.CeilToInt(Mathf.Sqrt(unitCount));
        int rows = Mathf.CeilToInt((float)unitCount / columns);

        Vector3[] positions = new Vector3[unitCount];

        for (int i = 0; i < unitCount; i++)
        {
            int row = i / columns;
            int col = i % columns;

            float xOffset = (col - columns / 2f) * unitSpacing;
            float yOffset = -row * unitSpacing;

            positions[i] = spawnPoint + new Vector3(xOffset, yOffset, 0);
        }

        return positions;
    }

    private Vector3[] CalculateLineFormation(Vector3 spawnPoint, int unitCount)
    {
        Vector3[] positions = new Vector3[unitCount];

        for (int i = 0; i < unitCount; i++)
        {
            float xOffset = (i - (unitCount - 1) / 2f) * unitSpacing;
            positions[i] = spawnPoint + new Vector3(xOffset, 0, 0);
        }

        return positions;
    }

    private Vector3[] CalculateVShapeFormation(Vector3 spawnPoint, int unitCount)
    {
        Vector3[] positions = new Vector3[unitCount];

        int leftCount = unitCount / 2;
        int rightCount = unitCount - leftCount;

        for (int i = 0; i < leftCount; i++)
        {
            float xOffset = -(i + 1) * unitSpacing;
            float yOffset = i * unitSpacing;
            positions[i] = spawnPoint + new Vector3(xOffset, yOffset, 0);
        }

        positions[leftCount] = spawnPoint;

        for (int i = 0; i < rightCount - 1; i++)
        {
            float xOffset = (i + 1) * unitSpacing;
            float yOffset = (i + 1) * unitSpacing;
            positions[leftCount + 1 + i] = spawnPoint + new Vector3(xOffset, yOffset, 0);
        }

        return positions;
    }

    public void RegisterToFormation(int formationId, Transform unit)
    {
        if (!activeFormations.ContainsKey(formationId))
        {
            activeFormations[formationId] = new List<Transform>();
        }
        activeFormations[formationId].Add(unit);
    }

    public void UnregisterFromFormation(int formationId, Transform unit)
    {
        if (activeFormations.ContainsKey(formationId))
        {
            activeFormations[formationId].Remove(unit);
            if (activeFormations[formationId].Count == 0)
            {
                activeFormations.Remove(formationId);
            }
        }
    }

    public void MoveFormation(int formationId, Vector3 targetPosition)
    {
        if (!activeFormations.ContainsKey(formationId)) return;

        List<Transform> units = activeFormations[formationId];
        Vector3[] formationPositions = CalculateFormationPositions(targetPosition, units.Count, defaultFormation);

        for (int i = 0; i < units.Count; i++)
        {
            UnitMovement movement = units[i].GetComponent<UnitMovement>();
            if (movement != null)
            {
                movement.SetTargetPosition(formationPositions[i], formationMoveSpeed);
            }
        }
    }

    public void ReorganizeFormation(int formationId)
    {
        if (!activeFormations.ContainsKey(formationId)) return;

        List<Transform> units = activeFormations[formationId];
        if (units.Count == 0) return;

        Transform leader = units[0];
        Vector3 centerPoint = GetFormationCenter(formationId);
        Vector3[] formationPositions = CalculateFormationPositions(centerPoint, units.Count, defaultFormation);

        for (int i = 0; i < units.Count; i++)
        {
            UnitMovement movement = units[i].GetComponent<UnitMovement>();
            if (movement != null)
            {
                movement.SetTargetPosition(formationPositions[i], formationMoveSpeed);
            }
        }
    }

    private Vector3 GetFormationCenter(int formationId)
    {
        if (!activeFormations.ContainsKey(formationId)) return Vector3.zero;

        List<Transform> units = activeFormations[formationId];
        Vector3 center = Vector3.zero;
        foreach (Transform unit in units)
        {
            center += unit.position;
        }
        return center / units.Count;
    }

    public int CreateNewFormation(Transform spawnPoint, int unitCount)
    {
        int formationId = Guid.NewGuid().GetHashCode();
        Vector3[] positions = CalculateFormationPositions(spawnPoint.position, unitCount, defaultFormation);

        activeFormations[formationId] = new List<Transform>();
        
        return formationId;
    }
}

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float arrivalThreshold = 0.1f;
    [SerializeField] private bool usePathfinding = true;

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private bool hasTarget;
    private int formationId = -1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.freezeRotation = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        if (hasTarget)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > arrivalThreshold)
        {
            rb.velocity = direction * moveSpeed;
            
            if (direction != Vector2.zero)
            {
                transform.up = direction;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            hasTarget = false;
        }
    }

    public void SetTargetPosition(Vector3 position, float speed)
    {
        targetPosition = position;
        moveSpeed = speed;
        hasTarget = true;
    }

    public void SetFormationId(int id)
    {
        formationId = id;
    }

    void OnDestroy()
    {
        if (formationId >= 0)
        {
            UnitFormationSystem.Instance?.UnregisterFromFormation(formationId, transform);
        }
    }
}
