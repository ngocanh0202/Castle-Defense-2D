using System;
using System.Collections.Generic;
using Common2D;
using Common2D.CreateGameObject2D;
using Common2D.EventMouse2D;
using Common2D.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;

public enum InputType
{
    Move,
    Stop,
    Attack,
    SetBulletStrategy,
    ReloadAmmo,
    OpenInventory,
    OnSetWeaponRotation,
    TowerSelect,
    TowerDrag
}

public class InputManager : Singleton<InputManager>
{
    [Header("Input Mode")]
    [SerializeField] private bool rtsMode = true;
    [SerializeField] private bool canInput;
    [SerializeField] private Quaternion weaponRotation;

    [Header("Touch Settings")]
    [SerializeField] private float dragThreshold = 10f;

    public event Action<Vector2> OnMove;
    public event Action<bool> OnStop;
    public event Action OnAttack;
    public event Action<KeyGuns> OnSetBulletStrategy;
    public event Action<Action<Transform>> OnSetWeaponRotation;
    public event Action OnReloadAmmo;
    public event Action OnOpenInventory;
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

        if (rtsMode)
        {
            inputTypes.Remove(InputType.Move);
            inputTypes.Remove(InputType.Stop);
            inputTypes.Remove(InputType.Attack);
            inputTypes.Remove(InputType.SetBulletStrategy);
            inputTypes.Remove(InputType.ReloadAmmo);
        }
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

        HandleKeyboardInput();
        HandleTouchInput();
    }

    private void HandleKeyboardInput()
    {
        if (!rtsMode && IsInputTypeCanBeUsed(InputType.Attack))
        {
            OnSetWeaponRotation?.Invoke(transformObj => EventMouse2D.LookAtMouse2D(transformObj, 5f));
        }

        if (!rtsMode && IsInputTypeCanBeUsed(InputType.Move))
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            if ((moveX != 0 || moveY != 0))
            {
                OnStop?.Invoke(false);
                Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
                OnMove?.Invoke(moveDirection);
            }
            else
            {
                OnStop?.Invoke(true);
            }
        }

        if (!rtsMode && Input.GetMouseButtonDown(0) && IsInputTypeCanBeUsed(InputType.Attack))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            OnAttack?.Invoke();
        }

        if (!rtsMode && Input.GetKeyDown(KeyCode.I) && IsInputTypeCanBeUsed(InputType.OpenInventory))
        {
            OnOpenInventory?.Invoke();
        }

        if (!rtsMode && IsInputTypeCanBeUsed(InputType.Attack))
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnSetBulletStrategy?.Invoke(KeyGuns.BaseBullet);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnSetBulletStrategy?.Invoke(KeyGuns.RatlingGunBullet);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OnSetBulletStrategy?.Invoke(KeyGuns.Boom);
            }

        if (!rtsMode && Input.GetKeyDown(KeyCode.R) && IsInputTypeCanBeUsed(InputType.ReloadAmmo))
        {
            OnReloadAmmo?.Invoke();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
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

    public void SetRTSMode(bool enabled)
    {
        rtsMode = enabled;
        if (rtsMode)
        {
            inputTypes.Remove(InputType.Move);
            inputTypes.Remove(InputType.Stop);
            inputTypes.Remove(InputType.Attack);
            inputTypes.Remove(InputType.SetBulletStrategy);
            inputTypes.Remove(InputType.ReloadAmmo);
        }
        else
        {
            inputTypes.Add(InputType.Move);
            inputTypes.Add(InputType.Stop);
            inputTypes.Add(InputType.Attack);
            inputTypes.Add(InputType.SetBulletStrategy);
            inputTypes.Add(InputType.ReloadAmmo);
        }
    }
}
