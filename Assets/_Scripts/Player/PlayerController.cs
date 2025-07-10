using System.Collections;
using System.Collections.Generic;
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
    Rigidbody2D rb;
    Gun gun;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        gun = GetComponentInChildren<Gun>();

        InputManager.Instance.OnMove += OnMove;
        InputManager.Instance.OnAttack += OnAttack;
        InputManager.Instance.OnOpenInventory += OnOpenInventory;
        InputManager.Instance.OnStop += OnStop;
        InputManager.Instance.OnSetBulletStrategy += OnSetBulletStrategy;
        InputManager.Instance.OnReloadAmmo += OnReloadAmmo;

    }

    void OnMove(Vector2 moveDirection)
    {
        if (IsStop)
        {
            Debug.Log("Player is stopped, not moving.");
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = moveDirection * moveSpeed;
    }

    void OnAttack()
    {
        gun.Attack();
    }

    void OnSetBulletStrategy(KeyOfObjPooler strategyType)
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
