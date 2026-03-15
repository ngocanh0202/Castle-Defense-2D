using System;
using System.Collections.Generic;
using UnityEngine;
using Common2D;

public class TowerSelectionManager : Singleton<TowerSelectionManager>
{
    [Header("Tower Selection")]
    [SerializeField] private TowerInteraction selectedTower;
    [SerializeField] private List<TowerInteraction> allTowers = new List<TowerInteraction>();

    [Header("Unit Spawn Panel")]
    [SerializeField] private GameObject spawnPanel;
    [SerializeField] private Transform spawnPanelPosition;

    protected override void Awake()
    {
        base.Awake();
        FindAllTowers();
    }

    void Start()
    {
        SetupSpawnPanel();
        InputManager.Instance.OnTowerTap += HandleTowerTap;
    }

    private void FindAllTowers()
    {
        TowerDefenseBehaviour[] towers = FindObjectsOfType<TowerDefenseBehaviour>();
        foreach (var tower in towers)
        {
            TowerInteraction interaction = tower.GetComponent<TowerInteraction>();
            if (interaction != null)
            {
                allTowers.Add(interaction);
            }
        }
    }

    private void SetupSpawnPanel()
    {
        if (spawnPanel == null)
        {
            spawnPanel = CreateSpawnPanel();
        }
        spawnPanel.SetActive(false);
    }

    private GameObject CreateSpawnPanel()
    {
        GameObject panel = new GameObject("UnitSpawnPanel");
        panel.transform.SetParent(transform);

        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            panel.transform.SetParent(canvas.transform);
        }

        SpriteRenderer sr = panel.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        panel.transform.localScale = new Vector3(3f, 2f, 1f);

        CreateSpawnButtons(panel);

        return panel;
    }

    private void CreateSpawnButtons(GameObject panel)
    {
        GameObject soldierButton = CreateUnitButton(panel.transform, "Soldier", new Vector2(-0.8f, 0));
        GameObject archerButton = CreateUnitButton(panel.transform, "Archer", new Vector2(0.8f, 0));
    }

    private GameObject CreateUnitButton(Transform parent, string unitName, Vector2 localPosition)
    {
        GameObject button = new GameObject(unitName + "Button");
        button.transform.SetParent(parent);
        button.transform.localPosition = localPosition;
        button.transform.localScale = Vector3.one * 0.8f;

        SpriteRenderer sr = button.AddComponent<SpriteRenderer>();
        sr.color = GetUnitColor(unitName);

        BoxCollider2D col = button.AddComponent<BoxCollider2D>();
        col.size = Vector2.one;

        UnitSpawnButton spawnButton = button.AddComponent<UnitSpawnButton>();
        spawnButton.UnitTypeName = unitName;
        spawnButton.OnButtonClicked += HandleUnitButtonClicked;

        return button;
    }

    private Color GetUnitColor(string unitName)
    {
        switch (unitName)
        {
            case "Soldier": return Color.blue;
            case "Archer": return Color.green;
            default: return Color.white;
        }
    }

    private void HandleTowerTap(Vector2 worldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        
        if (hit.collider != null)
        {
            TowerInteraction tower = hit.collider.GetComponent<TowerInteraction>();
            if (tower != null)
            {
                SelectTower(tower);
                return;
            }
        }

        DeselectCurrentTower();
    }

    private void SelectTower(TowerInteraction tower)
    {
        if (selectedTower != null && selectedTower != tower)
        {
            selectedTower.Deselect();
        }

        selectedTower = tower;
        tower.Select();
        ShowSpawnPanel(tower);
    }

    private void DeselectCurrentTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Deselect();
            selectedTower = null;
        }
        HideSpawnPanel();
    }

    private void ShowSpawnPanel(TowerInteraction tower)
    {
        if (spawnPanel != null)
        {
            spawnPanel.SetActive(true);
            spawnPanel.transform.position = tower.transform.position + Vector3.up * 2.5f;
            spawnPanel.transform.localScale = new Vector3(3f, 2f, 1f);
        }
    }

    private void HideSpawnPanel()
    {
        if (spawnPanel != null)
        {
            spawnPanel.SetActive(false);
        }
    }

    private void HandleUnitButtonClicked(string unitTypeName)
    {
        if (selectedTower == null) return;

        if (System.Enum.TryParse<UnitType>(unitTypeName, out UnitType unitType))
        {
            selectedTower.SpawnUnit(unitType);
        }
    }

    public TowerInteraction GetSelectedTower()
    {
        return selectedTower;
    }

    public void RefreshTowerList()
    {
        allTowers.Clear();
        FindAllTowers();
    }
}

public class UnitSpawnButton : MonoBehaviour
{
    public string UnitTypeName;
    public event Action<string> OnButtonClicked;

    void OnMouseDown()
    {
        OnButtonClicked?.Invoke(UnitTypeName);
    }
}
