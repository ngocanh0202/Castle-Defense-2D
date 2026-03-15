using UnityEngine;

[RequireComponent(typeof(TowerDefenseBehaviour))]
[RequireComponent(typeof(TowerInteraction))]
public class UnitTowerBehaviour : MonoBehaviour
{
    [Header("Unit Tower Settings")]
    [SerializeField] private float spawnCooldown = 5f;
    [SerializeField] private UnitType defaultUnitType = UnitType.Soldier;

    private TowerDefenseBehaviour towerDefenseBehaviour;
    private TowerInteraction towerInteraction;
    private ReceiveDamageBehaviour receiveDamageBehaviour;

    public TowerInteraction TowerInteraction => towerInteraction;
    public bool IsSelected => towerInteraction.IsSelected;

    void Awake()
    {
        towerDefenseBehaviour = GetComponent<TowerDefenseBehaviour>();
        towerInteraction = GetComponent<TowerInteraction>();
        receiveDamageBehaviour = GetComponent<ReceiveDamageBehaviour>();

        towerInteraction.SetCooldown(spawnCooldown);
    }

    void OnEnable()
    {
        towerInteraction.OnTowerSelected += HandleTowerSelected;
        towerInteraction.OnTowerDeselected += HandleTowerDeselected;
    }

    void OnDisable()
    {
        towerInteraction.OnTowerSelected -= HandleTowerSelected;
        towerInteraction.OnTowerDeselected -= HandleTowerDeselected;
    }

    private void HandleTowerSelected(TowerInteraction tower)
    {
    }

    private void HandleTowerDeselected(TowerInteraction tower)
    {
    }

    public bool SpawnUnit(UnitType unitType = UnitType.Soldier)
    {
        return towerInteraction.SpawnUnit(unitType);
    }

    public bool CanSpawnUnit()
    {
        return towerInteraction.CanSpawn;
    }

    public float GetCooldownRemaining()
    {
        return towerInteraction.CurrentCooldown;
    }
}
