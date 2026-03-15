using UnityEngine;

[RequireComponent(typeof(TowerDefenseBehaviour))]
public class EnemyTowerBehaviour : MonoBehaviour
{
    [Header("Enemy Tower Settings")]
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private UnitType enemyUnitType = UnitType.Soldier;
    [SerializeField] private bool autoSpawn = true;

    private TowerDefenseBehaviour towerDefenseBehaviour;
    private ReceiveDamageBehaviour receiveDamageBehaviour;
    private float spawnTimer;

    public float SpawnInterval
    {
        get => spawnInterval;
        set => spawnInterval = value;
    }

    public bool AutoSpawn
    {
        get => autoSpawn;
        set => autoSpawn = value;
    }

    void Awake()
    {
        towerDefenseBehaviour = GetComponent<TowerDefenseBehaviour>();
        receiveDamageBehaviour = GetComponent<ReceiveDamageBehaviour>();
        spawnTimer = 0f;
    }

    void OnEnable()
    {
        spawnTimer = 0f;
    }

    void Update()
    {
        if (!autoSpawn) return;
        if (!receiveDamageBehaviour.IsAlive) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    public void SpawnEnemy()
    {
        if (!autoSpawn) return;
        if (!receiveDamageBehaviour.IsAlive) return;

        GameObject unitPrefab = GetEnemyUnitPrefab(enemyUnitType);
        if (unitPrefab != null)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            GameObject unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            unit.tag = "Enemy";
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return transform.position + (Vector3.down * 2f);
    }

    private GameObject GetEnemyUnitPrefab(UnitType unitType)
    {
        string prefabName = "Enemy" + unitType.ToString();
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        if (prefab == null)
        {
            prefab = Resources.Load<GameObject>("EnemyUnit");
        }
        return prefab;
    }

    public void ResetSpawnTimer()
    {
        spawnTimer = 0f;
    }
}
