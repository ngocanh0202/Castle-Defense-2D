using System;
using System.Collections.Generic;
using Common2D;
using Common2D.EventMouse2D;
using Common2D.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputType
{
    TowerSelect,
    TowerDrag,
    ToggleBuildMode
}

public class InputManager : Singleton<InputManager>
{
    [Header("Input Settings")]
    [SerializeField] private bool canInput = true;

    [Header("Touch Settings")]
    [SerializeField] private float dragThreshold = 10f;

    public event Action<bool> OnToggleBuildMode;
    public event Action<Vector2> OnTowerTap;
    public event Action<Vector2, Vector2> OnTowerDrag;

    public List<InputType> inputTypes;

    private Vector2 touchStartPosition;
    private bool isDragging;
    private bool isTowerDragging;
    private bool isUITouch;
    private GameObject draggedTower;

    protected override void Awake()
    {
        base.Awake();
        inputTypes = new List<InputType>((InputType[])Enum.GetValues(typeof(InputType)));
        canInput = true;
    }

    void Start()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameManagerState newState)
    {
        if (newState == GameManagerState.GameOver)
        {
            DisableAllInput();
        }
    }

    void Update()
    {
        if (!canInput)
            return;

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            touchStartPosition = Input.mousePosition;
            isDragging = false;
            isTowerDragging = false;
            isUITouch = false;

            if (IsInputTypeCanBeUsed(InputType.TowerSelect))
            {
                Vector2 worldPosition = GetWorldPosition(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Tower"))
                {
                    draggedTower = hit.collider.gameObject;
                }
            }
        }

        if (Input.GetMouseButton(0) && draggedTower != null)
        {
            float dragDistance = Vector2.Distance(Input.mousePosition, touchStartPosition);
            if (dragDistance > dragThreshold)
            {
                isDragging = true;

                if (IsInputTypeCanBeUsed(InputType.TowerDrag))
                {
                    isTowerDragging = true;
                    Vector2 worldStart = GetWorldPosition(touchStartPosition);
                    Vector2 worldCurrent = GetWorldPosition(Input.mousePosition);
                    OnTowerDrag?.Invoke(worldStart, worldCurrent);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isUITouch && !isDragging && !isTowerDragging && IsInputTypeCanBeUsed(InputType.TowerSelect))
            {
                Vector2 worldPosition = GetWorldPosition(Input.mousePosition);
                OnTowerTap?.Invoke(worldPosition);
            }

            isDragging = false;
            isTowerDragging = false;
            isUITouch = false;
            draggedTower = null;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPosition = touch.position;
                    isDragging = false;
                    isTowerDragging = false;
                    isUITouch = false;

                    if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        isUITouch = true;
                        return;
                    }

                    if (IsInputTypeCanBeUsed(InputType.TowerSelect))
                    {
                        Vector2 worldPosition = GetWorldPosition(touch.position);
                        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                        if (hit.collider != null && hit.collider.CompareTag("Tower"))
                        {
                            draggedTower = hit.collider.gameObject;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    float dragDistance = Vector2.Distance(touch.position, touchStartPosition);
                    if (dragDistance > dragThreshold)
                    {
                        isDragging = true;

                        if (draggedTower != null && IsInputTypeCanBeUsed(InputType.TowerDrag))
                        {
                            isTowerDragging = true;
                            Vector2 worldStart = GetWorldPosition(touchStartPosition);
                            Vector2 worldCurrent = GetWorldPosition(touch.position);
                            OnTowerDrag?.Invoke(worldStart, worldCurrent);
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    if (!isUITouch && !isDragging && !isTowerDragging && IsInputTypeCanBeUsed(InputType.TowerSelect))
                    {
                        Vector2 worldPosition = GetWorldPosition(touch.position);
                        OnTowerTap?.Invoke(worldPosition);
                    }

                    isDragging = false;
                    isTowerDragging = false;
                    isUITouch = false;
                    draggedTower = null;
                    break;
            }
        }
    }

    private Vector2 GetWorldPosition(Vector2 screenPosition)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            return cam.ScreenToWorldPoint(screenPosition);
        }
        return screenPosition;
    }

    public void DisableAllInput()
    {
        inputTypes.Clear();
    }

    public void DisableInput(params InputType[] inputDisableTypes)
    {
        foreach (var inputTypeDisable in inputDisableTypes)
        {
            inputTypes.Remove(inputTypeDisable);
        }
    }

    public void EnableInput(params InputType[] inputTypes)
    {
        if (inputTypes == null || inputTypes.Length == 0)
        {
            this.inputTypes = new List<InputType>((InputType[])Enum.GetValues(typeof(InputType)));
        }
        else
        {
            this.inputTypes = new List<InputType>(inputTypes);
        }
    }

    private bool IsInputTypeCanBeUsed(InputType inputType)
    {
        return inputTypes.Contains(inputType);
    }

    public void ToggleBuildMode()
    {
        if (!IsInputTypeCanBeUsed(InputType.ToggleBuildMode))
            return;

        OnToggleBuildMode?.Invoke(true);
    }

    public void SetBuildMode(bool enabled)
    {
        if (enabled)
        {
            if (!inputTypes.Contains(InputType.ToggleBuildMode))
            {
                inputTypes.Add(InputType.ToggleBuildMode);
            }
        }
        else
        {
            inputTypes.Remove(InputType.ToggleBuildMode);
        }
    }
}
