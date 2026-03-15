using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnButtonUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button spawnButton;
    [SerializeField] private GameObject buttonContainer;

    [Header("Settings")]
    [SerializeField] private UnitType spawnUnitType = UnitType.Soldier;

    private UnitTowerBehaviour selectedTower;

    void Start()
    {
        HideSpawnButton();
        
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(OnSpawnButtonClicked);
        }
    }

    void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTowerTap += HandleTowerTap;
        }
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnTowerTap -= HandleTowerTap;
        }
    }

    private void HandleTowerTap(Vector2 worldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        
        if (hit.collider != null)
        {
            UnitTowerBehaviour tower = hit.collider.GetComponent<UnitTowerBehaviour>();
            if (tower != null)
            {
                SelectTower(tower);
            }
            else
            {
                DeselectTower();
            }
        }
        else
        {
            DeselectTower();
        }
    }

    private void SelectTower(UnitTowerBehaviour tower)
    {
        selectedTower = tower;
        ShowSpawnButton();
        UpdateButtonState();
    }

    private void DeselectTower()
    {
        selectedTower = null;
        HideSpawnButton();
    }

    private void ShowSpawnButton()
    {
        if (buttonContainer != null)
        {
            buttonContainer.SetActive(true);
        }
    }

    private void HideSpawnButton()
    {
        if (buttonContainer != null)
        {
            buttonContainer.SetActive(false);
        }
    }

    private void UpdateButtonState()
    {
        if (spawnButton != null && selectedTower != null)
        {
            spawnButton.interactable = selectedTower.CanSpawnUnit();
        }
    }

    void Update()
    {
        if (selectedTower != null)
        {
            UpdateButtonState();
        }
    }

    private void OnSpawnButtonClicked()
    {
        if (selectedTower != null)
        {
            bool success = selectedTower.SpawnUnit(spawnUnitType);
            if (success)
            {
                Debug.Log("Unit spawned successfully!");
            }
        }
    }

    public void SetSpawnUnitType(UnitType unitType)
    {
        spawnUnitType = unitType;
    }
}
