using System;
using System.Collections.Generic;
using Common2D;
using Common2D.CreateGameObject2D;
using Common2D.EventMouse2D;
using UnityEngine;

public enum InputType
{
    Move,
    Stop,
    Attack,
    SetBulletStrategy,
    ReloadAmmo,
    OpenInventory
}

public class InputManager : Singleton<InputManager>
{
    [SerializeField] private bool canInput;
    public event Action<float, float> OnMove;
    public event Action<bool> OnStop;
    public event Action OnAttack;
    public event Action<KeyOfObjPooler> OnSetBulletStrategy;
    public event Action OnReloadAmmo;
    public event Action OnOpenInventory;
    public List<InputType> inputTypes;
    void Start()
    {
        inputTypes = new List<InputType>((InputType[])Enum.GetValues(typeof(InputType)));
        canInput = true;
        PopupText popupText = ResourcesManager.GetPopupTextPrefab().GetComponent<PopupText>();
        ObjectPooler.InitObjectPooler<PopupText>(
            KeyOfObjPooler.PopupText.ToString(),
            3,
            popupText,
            null
        );
    }
    void Update()
    {
        if (!canInput)
            return;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        if ((moveX != 0 || moveY != 0) && IsInputTypeCanBeUsed(InputType.Move))
        {
            OnStop?.Invoke(false);
            OnMove?.Invoke(moveX, moveY);
        }
        else
        {
            OnStop?.Invoke(true);
        }

        if (Input.GetMouseButtonDown(0) && IsInputTypeCanBeUsed(InputType.Attack))
        {
            OnAttack?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.I) && IsInputTypeCanBeUsed(InputType.OpenInventory))
        {
            OnOpenInventory?.Invoke();
        }


        if( IsInputTypeCanBeUsed(InputType.Attack))
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnSetBulletStrategy?.Invoke(KeyOfObjPooler.BaseBullet);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnSetBulletStrategy?.Invoke(KeyOfObjPooler.RatlingGunBullet);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                OnSetBulletStrategy?.Invoke(KeyOfObjPooler.Boom);
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
