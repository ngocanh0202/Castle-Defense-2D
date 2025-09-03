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
    OnSetWeaponRotation
}

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private bool canInput;
    [SerializeField] private Quaternion weaponRotation;
    public event Action<Vector2> OnMove;
    public event Action<bool> OnStop;
    public event Action OnAttack;
    public event Action<KeyGuns> OnSetBulletStrategy;
    public event Action<Action<Transform>> OnSetWeaponRotation;
    public event Action OnReloadAmmo;
    public event Action OnOpenInventory;
    public List<InputType> inputTypes;
    protected override void Awake()
    {
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

        if (IsInputTypeCanBeUsed(InputType.Attack))
        {
            OnSetWeaponRotation?.Invoke(
                (TransformObj) => EventMouse2D.LookAtMouse2D(TransformObj, 5f));
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        if ((moveX != 0 || moveY != 0) && IsInputTypeCanBeUsed(InputType.Move))
        {
            OnStop?.Invoke(false);
            Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
            OnMove?.Invoke(moveDirection);
        }
        else
        {
            OnStop?.Invoke(true);
        }

        if (Input.GetMouseButtonDown(0) && IsInputTypeCanBeUsed(InputType.Attack))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            OnAttack?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.I) && IsInputTypeCanBeUsed(InputType.OpenInventory))
        {
            OnOpenInventory?.Invoke();
        }


        if( IsInputTypeCanBeUsed(InputType.Attack))
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

        if (Input.GetKeyDown(KeyCode.R) && IsInputTypeCanBeUsed(InputType.ReloadAmmo))
        {
            OnReloadAmmo?.Invoke();
        }


        if (Input.GetKeyDown(KeyCode.V) && IsInputTypeCanBeUsed(InputType.Attack))
        {
            CreateGameObject.CreateTextPopup("Hello World", EventMouse2D.GetPositionOnMouse2D(), Color.white, null);
        }
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
}
