using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Soldier,
    Archer,
    Knight
}

[RequireComponent(typeof(Collider2D))]
public class TowerInteraction : MonoBehaviour
{
    [Header("Tower Interaction")]
    [SerializeField] private TowerDefenseBehaviour towerBehaviour;
    [SerializeField] private bool isSelected;
    [SerializeField] private GameObject selectionIndicator;

    [Header("Unit Spawning")]
    [SerializeField] private float spawnCooldown = 5f;
    [SerializeField] private float currentCooldown;
    [SerializeField] private List<UnitType> availableUnits;

    private static Sprite cachedCircleSprite;

    public event Action<TowerInteraction> OnTowerSelected;
    public event Action<TowerInteraction> OnTowerDeselected;
    public event Action<TowerInteraction, UnitType> OnUnitSpawned;

    public TowerDefenseBehaviour TowerBehaviour => towerBehaviour;
    public bool IsSelected => isSelected;
    public float CurrentCooldown => currentCooldown;
    public bool CanSpawn => currentCooldown <= 0;

    void Awake()
    {
        towerBehaviour = GetComponent<TowerDefenseBehaviour>();
        InitializeAvailableUnits();
    }

    void Start()
    {
        SetupSelectionIndicator();
    }

    void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void InitializeAvailableUnits()
    {
        availableUnits = new List<UnitType> { UnitType.Soldier };
    }

    private void SetupSelectionIndicator()
    {
        if (selectionIndicator == null)
        {
            GameObject indicator = new GameObject("SelectionIndicator");
            indicator.transform.SetParent(transform);
            indicator.transform.localPosition = Vector3.zero;
            SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
            sr.sprite = CreateCircleSprite();
            sr.color = new Color(0f, 1f, 0f, 0.3f);
            sr.sortingOrder = -1;
            indicator.transform.localScale = Vector3.one * 3f;
            selectionIndicator = indicator;
        }
        selectionIndicator.SetActive(false);
    }

    private Sprite CreateCircleSprite()
    {
        if (cachedCircleSprite != null)
            return cachedCircleSprite;

        Texture2D texture = new Texture2D(32, 32);
        for (int y = 0; y < 32; y++)
        {
            for (int x = 0; x < 32; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(15.5f, 15.5f));
                if (dist < 15f)
                    texture.SetPixel(x, y, Color.white);
                else
                    texture.SetPixel(x, y, Color.clear);
            }
        }
        texture.Apply();
        cachedCircleSprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.one * 0.5f);
        return cachedCircleSprite;
    }

    public void Select()
    {
        if (!isSelected)
        {
            isSelected = true;
            selectionIndicator.SetActive(true);
            OnTowerSelected?.Invoke(this);
        }
    }

    public void Deselect()
    {
        if (isSelected)
        {
            isSelected = false;
            selectionIndicator.SetActive(false);
            OnTowerDeselected?.Invoke(this);
        }
    }

    public bool SpawnUnit(UnitType unitType)
    {
        if (!CanSpawn)
        {
            Debug.Log($"Cannot spawn {unitType}: cooldown not ready ({currentCooldown:F1}s remaining)");
            return false;
        }

        if (!availableUnits.Contains(unitType))
        {
            Debug.Log($"Cannot spawn {unitType}: unit type not available for this tower");
            return false;
        }

        SpawnUnitAtPosition(unitType, GetSpawnPosition());
        currentCooldown = spawnCooldown;
        OnUnitSpawned?.Invoke(this, unitType);
        return true;
    }

    private Vector3 GetSpawnPosition()
    {
        return transform.position + (Vector3.down * 2f);
    }

    private void SpawnUnitAtPosition(UnitType unitType, Vector3 position)
    {
        GameObject unitPrefab = GetUnitPrefab(unitType);
        if (unitPrefab != null)
        {
            Instantiate(unitPrefab, position, Quaternion.identity);
        }
    }

    private GameObject GetUnitPrefab(UnitType unitType)
    {
        string prefabName = unitType.ToString() + "Unit";
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>("AlliedUnit");
        }
        return prefab;
    }

    public void SetCooldown(float cooldown)
    {
        spawnCooldown = cooldown;
    }

    public List<UnitType> GetAvailableUnits()
    {
        return new List<UnitType>(availableUnits);
    }
}
