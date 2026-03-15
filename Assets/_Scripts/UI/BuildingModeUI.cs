using UnityEngine;
using UnityEngine.UI;

public class BuildingModeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button buildModeButton;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Text buttonText;

    [Header("Visual Settings")]
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Color inactiveColor = Color.gray;
    [SerializeField] private string activeText = "BUILD ON";
    [SerializeField] private string inactiveText = "BUILD OFF";

    private bool isBuildModeActive = false;

    void Start()
    {
        if (buildModeButton == null)
        {
            buildModeButton = GetComponent<Button>();
        }

        if (buildModeButton != null)
        {
            buildModeButton.onClick.AddListener(ToggleBuildMode);
        }

        UpdateButtonVisuals();
    }

    void OnEnable()
    {
        if (TowerGridManager.Instance != null)
        {
            TowerGridManager.Instance.onSetBuildingMode += HandleBuildModeChanged;
        }
    }

    void OnDisable()
    {
        if (TowerGridManager.Instance != null)
        {
            TowerGridManager.Instance.onSetBuildingMode -= HandleBuildModeChanged;
        }
    }

    private void ToggleBuildMode()
    {
        isBuildModeActive = !isBuildModeActive;
        TowerGridManager.Instance?.SetBuildMode(isBuildModeActive);
        UpdateButtonVisuals();
    }

    private void HandleBuildModeChanged(bool isActive)
    {
        isBuildModeActive = isActive;
        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isBuildModeActive ? activeColor : inactiveColor;
        }

        if (buttonText != null)
        {
            buttonText.text = isBuildModeActive ? activeText : inactiveText;
        }
    }

    public void SetBuildMode(bool enabled)
    {
        isBuildModeActive = enabled;
        TowerGridManager.Instance?.SetBuildMode(enabled);
        UpdateButtonVisuals();
    }
}
