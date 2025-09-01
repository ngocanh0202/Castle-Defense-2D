using System;
using System.Collections;
using System.Collections.Generic;
using Common2D.Inventory;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float moveSpeed = 5f;
    [Header("Player Status")]
    [SerializeField] bool IsStop = true;
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Gun gun;
    [SerializeField] Transform gunTransform;
    [SerializeField] Camera mainCamera;
    void Start()
    {
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
        rb.velocity = moveDirection * moveSpeed;
    }

    void OnAttack()
    {
        if(gun.isAttacking) return;
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
        }
    }
}
