
using System;
using Common2D.Singleton;
using UnityEngine;

public enum TowerDirection
{
    Top = 0,
    Right = 1,
    Bottom = 2,
    Left = 3,
    TopRight = 4,
    BottomRight = 5,
    BottomLeft = 6,
    TopLeft = 7
}

public class TowerTile
{
    public bool isOccupied;
    public bool isBuildable;
    public Vector2Int gridPosition;
    public TowerDefenseBehaviour currentTower;
    string[] directions;
    private readonly GridCustom<TowerTile> gridCustom;

    private static readonly Vector2Int[] directionOffsets =
    {
        new Vector2Int(0, 1),   // Top
        new Vector2Int(1, 0),   // Right
        new Vector2Int(0, -1),  // Bottom
        new Vector2Int(-1, 0),  // Left
        new Vector2Int(1, 1),   // TopRight
        new Vector2Int(1, -1),  // BottomRight
        new Vector2Int(-1, -1), // BottomLeft
        new Vector2Int(-1, 1)   // TopLeft
    };

    public TowerTile(GridCustom<TowerTile> gridCustom, Vector2Int position, bool isBuildable)
    {
        this.gridCustom = gridCustom;
        gridPosition = position;
        this.isBuildable = isBuildable;
    }

    public bool TryPlaceBuilding(TowerDefenseBehaviour tower)
    {
        var towerStat = tower.GetComponent<TowerStat>();
        string[] tempDirectionStr = towerStat.GetStatValue<string>(TowerStatType.Direction).Split(";");
        
        if (tempDirectionStr.Length != 8)
        {
            Debug.LogError("Invalid tower direction config.");
            return false;
        }

        if (!CanBuildTower(tempDirectionStr))
        {
            NotificationSystem.Instance.ShowNotification("Cannot place tower here!", NotificationType.Error, 2f);
            return false;
        }

        PlaceBuildingInternal(tower, tempDirectionStr);
        return true;
    }

    public void PlaceBuildingInternal(TowerDefenseBehaviour tower, string[] directionDistances)
    {
        tower.GetComponent<ReceiveDamageBehaviour>().OnDie += OnDestroy;

        isOccupied = true;
        currentTower = tower;
        currentTower.transform.position = gridCustom.GetMiddleInCell(gridPosition.x, gridPosition.y);
        directions = directionDistances;

        UpdateNeighborTilesBuildableState(false);
    }

    bool CanBuildTower(string[] tempDirection)
    {
        bool canBuild = (isOccupied || !isBuildable) ? false : true;

        for (int i = 0; i < tempDirection.Length; i++)
        {
            if (int.TryParse(tempDirection[i], out int distance) && distance > 0)
            {
                ReadTilesBuildableInDirection(i, distance, (tile) =>
                {
                    if (tile != null && (!tile.isBuildable || tile.isOccupied))
                    {
                        canBuild = false;
                        return true;
                    }
                    return false;
                });
            }
        }
        return canBuild;
    }

    private void UpdateNeighborTilesBuildableState(bool canBuild)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            if (int.TryParse(directions[i], out int distance) && distance > 0)
            {
                ReadTilesBuildableInDirection(i, distance, (tile)=>
                {
                    if (tile != null)
                    {
                        if (canBuild)
                            tile.SetCanBuildTower();
                        else
                            tile.SetCannotBuildTower();
                    }
                    return false;
                });
            }
        }
    }

    private void ReadTilesBuildableInDirection(int dirIndex, int distance, Func<TowerTile,bool> onSetTowerTile)
    {
        Vector2Int offset = directionOffsets[dirIndex];
        for (int step = 1; step <= distance; step++)
        {
            TowerTile tile = gridCustom.GetItemGrid(
                gridPosition.x + offset.x * step,
                gridPosition.y + offset.y * step
            );

            bool isLoopStop = onSetTowerTile(tile);
            if (isLoopStop)
                break;
        }
    }

    void OnDestroy(Transform transform)
    {
        if (currentTower != null)
        {
            SetCanBuildTower();
            UpdateNeighborTilesBuildableState(true);
            MonoBehaviour.Destroy(currentTower.gameObject);
            currentTower = null;
        }
    }

    public void SetCannotBuildTower() => isBuildable = false;

    public void SetCanBuildTower()
    {
        isOccupied = false;
        isBuildable = true;
    }
}