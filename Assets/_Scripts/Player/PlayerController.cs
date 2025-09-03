using System;
using Common2D;
using Common2D.CreateGameObject2D;
using Common2D.Inventory;
using Common2D.Singleton;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PlayerStat))]
public class PlayerController : Singleton<PlayerController>
{
    [Header("Player Stat")]
    [SerializeField] bool IsStop = true;
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Gun gun;
    [SerializeField] Transform gunTransform;
    [SerializeField] Camera mainCamera;
    [SerializeField] PlayerStat playerStat;
    [SerializeField] PlayerStat PlayerStat { get => playerStat; }
    void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        gun = GetComponentInChildren<Gun>();
        gunTransform = gun.transform;
        mainCamera = Camera.main;

        InputManager.Instance.OnMove += OnMove;
        InputManager.Instance.OnAttack += OnAttack;
        InputManager.Instance.OnOpenInventory += OnOpenInventory;
        InputManager.Instance.OnStop += OnStop;
        InputManager.Instance.OnSetBulletStrategy += OnSetBulletStrategy;
        InputManager.Instance.OnReloadAmmo += OnReloadAmmo;
        InputManager.Instance.OnSetWeaponRotation += OnSetWeaponRotation;

        gun.SetStrategy(KeyGuns.BaseBullet);
    }

    void Update()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        }
    }

    void OnSetWeaponRotation(Action<Transform> func)
    {
        func?.Invoke(gunTransform);
    }

    void OnMove(Vector2 moveDirection)
    {
        if (IsStop)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = moveDirection * playerStat.GetStatValue(PlayerStatType.Speed);
    }

    void OnAttack()
    {
        if (gun.isAttacking) return;
        gun.Attack();
    }

    void OnSetBulletStrategy(KeyGuns strategyType)
    {
        gun.SetStrategy(strategyType);
    }
    void OnReloadAmmo()
    {
        gun.ReloadAmmo(5, (redundantAmmo) =>
        {
            Debug.Log($"Reloaded ammo. Redundant ammo: {redundantAmmo}");
        });
    }
    void OnOpenInventory()
    {
        InventoryUI.Instance.ToggleInventory();
    }

    void OnStop(bool isStop)
    {
        IsStop = isStop;
        if (IsStop)
        {
            rb.velocity = Vector2.zero;
            // Hello
        }
    }

    public void IncreaseEXP(Transform transformEnemy)
    {
        PlayerStat.SetExperienceValue(1f, (exp)=>
        {
            CreateGameObject.CreateTextPopup($"EXP +{exp}", transform.position, Color.green);
        }, ()=>
        {
            CreateGameObject.CreateTextPopup($"Level Up!", transform.position, Color.yellow);
            NotificationSystem.Instance.ShowNotification("You Leveled Up!", NotificationType.Success);
        });
    }
}
