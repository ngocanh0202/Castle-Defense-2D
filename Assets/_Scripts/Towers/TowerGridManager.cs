using System;
using Common2D;
using Common2D.EventMouse2D;
using Common2D.Singleton;
using UnityEngine;
using Utilities;


public class TowerGridManager : Singleton<TowerGridManager>
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 startPosition;
    [SerializeField] bool isDebug = true;
    [SerializeField] private bool isBuildingMode = false;
    [SerializeField] private Material gridLineMaterial;

    private GridCustom<TowerTile> gridCustom;
    [SerializeField] Action<bool> onSetBuildingMode;
    [SerializeField] public Action onSetTowerEnemy;

    protected override void Awake()
    {
        base.Awake();
        gridCustom = new GridCustom<TowerTile>(
            width, height, cellSize, startPosition,
            (grid, x, y) => new TowerTile(grid, new Vector2Int(x, y), true)
        );
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameManagerState gameManagerState)
    {
        if (gameManagerState == GameManagerState.GameOver)
        {
            TowerDefenseBehaviour[] towers = FindObjectsByType<TowerDefenseBehaviour>(FindObjectsSortMode.None);
            foreach (var tower in towers)
            {
                tower.isStopAttack = true;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildingMode = !isBuildingMode;
            onSetBuildingMode?.Invoke(isBuildingMode);
        }

        if (isBuildingMode && Input.GetMouseButtonDown(1))
        {
            HandleSetTower();
        }
        if (isBuildingMode && Input.GetKeyDown(KeyCode.M))
        {
            HandleSetMainTower();
        }
        if (isBuildingMode && Input.GetMouseButtonDown(2))
        {
            HandleSetEnemyTower();
        }
    }

    void HandleSetEnemyTower()
    {
        GameObject gameObject = GameObject.Find("MainTower");
        if(gameObject == null)
        {
            NotificationSystem.Instance.ShowNotification("Please place the Main Tower first!", NotificationType.Error, 2f);
            return;
        }
        Vector3 mousePosition = EventMouse2D.GetPositionOnMouse();
        TowerTile tile = gridCustom.GetItemWithPosition(mousePosition);
        Transform enemyHolder = FindAnyObjectByType<EnemySpawnerManager>().transform.Find("SpawnPoints");
        TowerDefenseBehaviour newTower = Instantiate(ResourcesManager.GetEnemyTowerPrefab());
        newTower.gameObject.name = "Tower";
        newTower.keyGunType = KeyGuns.Boom;
        newTower.transform.SetParent(enemyHolder);
        newTower.GetComponent<TowerStat>().Reset();

        bool isPlaced = tile.TryPlaceBuilding(newTower);

        if (!isPlaced)
        {
            Destroy(newTower.gameObject);
        }
        else
        {
            onSetTowerEnemy?.Invoke();
        }
    }

    void HandleSetMainTower()
    {
        GameObject gameObject = GameObject.Find("MainTower");
        if(gameObject != null)
        {
            NotificationSystem.Instance.ShowNotification("Main Tower already placed!", NotificationType.Error, 2f);
            return;
        }
        Vector3 mousePosition = EventMouse2D.GetPositionOnMouse();
        TowerTile tile = gridCustom.GetItemWithPosition(mousePosition);

        TowerDefenseBehaviour newTower = Instantiate(ResourcesManager.GetPlayerTowerPrefab());
        newTower.tag = "Player";
        newTower.gameObject.name = "MainTower";
        newTower.keyGunType = KeyGuns.RatlingGunBullet;
        newTower.transform.SetParent(transform);
        newTower.GetComponent<TowerStat>().SetMainTowerStats();

        bool isPlaced = tile.TryPlaceBuilding(newTower);
        if (!isPlaced)
        {
            Destroy(newTower.gameObject);
        }
    }

    private void HandleSetTower()
    {
        GameObject gameObject = GameObject.Find("MainTower");
        if (gameObject == null)
        {
            NotificationSystem.Instance.ShowNotification("Please place the Main Tower first!", NotificationType.Error, 2f);
            return;
        }
        Vector3 mousePosition = EventMouse2D.GetPositionOnMouse();
        TowerTile tile = gridCustom.GetItemWithPosition(mousePosition);

        TowerDefenseBehaviour newTower = Instantiate(ResourcesManager.GetPlayerTowerPrefab());
        newTower.tag = "Player";
        newTower.gameObject.name = "Tower";
        newTower.keyGunType = KeyGuns.RatlingGunBullet;
        newTower.transform.SetParent(transform);
        newTower.GetComponent<TowerStat>().Reset();

        bool isPlaced = tile.TryPlaceBuilding(newTower);
        if (!isPlaced)
        {
            Destroy(newTower.gameObject);
        }
    }

    void OnRenderObject()
    {
        if (!isBuildingMode || gridLineMaterial == null) return;

        gridLineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(Color.green);

        gridCustom.ReadGridData((tile, x, y) =>
        {
            if (tile != null)
            {
                DrawRuntimeLine(GridUtils.GetPosition(x, y, cellSize, startPosition),
                                GridUtils.GetPosition(x + 1, y, cellSize, startPosition));
                DrawRuntimeLine(GridUtils.GetPosition(x, y, cellSize, startPosition),
                                GridUtils.GetPosition(x, y + 1, cellSize, startPosition));

                if (!tile.isBuildable)
                {
                    GL.Color(Color.red);
                    DrawRuntimeLine(GridUtils.GetPosition(x, y, cellSize, startPosition),
                                    GridUtils.GetPosition(x + 1, y + 1, cellSize, startPosition));
                    DrawRuntimeLine(GridUtils.GetPosition(x, y + 1, cellSize, startPosition),
                                    GridUtils.GetPosition(x + 1, y, cellSize, startPosition));
                    GL.Color(Color.green);
                }
            }
        });

        DrawRuntimeLine(GridUtils.GetPosition(width, height, cellSize, startPosition),
                        GridUtils.GetPosition(width, 0, cellSize, startPosition));
        DrawRuntimeLine(GridUtils.GetPosition(width, height, cellSize, startPosition),
                        GridUtils.GetPosition(0, height, cellSize, startPosition));

        GL.End();
    }

    private void DrawRuntimeLine(Vector3 start, Vector3 end)
    {
        GL.Vertex(start);
        GL.Vertex(end);
    }

    void OnDrawGizmos()
    {
        if (!isDebug) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DrawLineGrid(GridUtils.GetPosition(x, y, cellSize, startPosition),
                             GridUtils.GetPosition(x + 1, y, cellSize, startPosition));
                DrawLineGrid(GridUtils.GetPosition(x, y, cellSize, startPosition),
                             GridUtils.GetPosition(x, y + 1, cellSize, startPosition));
            }
        }

        // Border lines
        DrawLineGrid(GridUtils.GetPosition(width, height, cellSize, startPosition),
                     GridUtils.GetPosition(width, 0, cellSize, startPosition));
        DrawLineGrid(GridUtils.GetPosition(width, height, cellSize, startPosition),
                     GridUtils.GetPosition(0, height, cellSize, startPosition));
    }

    private void DrawLineGrid(Vector3 start, Vector3 end) => Gizmos.DrawLine(start, end);
}