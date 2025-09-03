using System;
using Common2D;
using Common2D.CreateGameObject2D;
using Common2D.EventMouse2D;
using Common2D.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Testing : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] TextMeshProUGUI textMeshProWeapon;
    [SerializeField] TextMeshProUGUI textMeshProBullet;
    [SerializeField] Transform transformHolderText;
    void Start()
    {
        gun.OnSetBulletStrategy += OnChangeTextBulletStrategy;
        gun.OnAmmoChanged += OnAmountChanged;
        textMeshProWeapon.text = KeyGuns.BaseBullet.ToString();
        textMeshProBullet.text = "10/10";

        InputManager.Instance.OnSetWeaponRotation += OnSetWeaponRotation;

        PopupText popupText = ResourcesManager.GetPopupTextPrefab().GetComponent<PopupText>();
        ObjectPooler.Instance.InitObjectPooler<PopupText>(
            KeyOfObjPooler.PopupText.ToString(),
            3,
            popupText
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ConfirmModalSystem.Instance.ShowConfirmModal("Are you sure reset game?", 2, OnListenerClickAccept: () =>
            {
                GameManager.Instance.ResetGame();
            });
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateGameObject.CreateCountdown(
                5, Vector3.zero, CountdownOptions.Seconds, transformParent: GameObject.Find(StringDefault.Canvas.ToString()).transform, onFinish: (textMeshPro) =>
                {
                    NotificationSystem.Instance.ShowNotification("Countdown Finished", NotificationType.Success);
                    Destroy(textMeshPro.gameObject);
                });
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CreateGameObject.CreateTextPopup(
                "Up", EventMouse2D.GetPositionOnMouse2D(), Color.yellow);
        }

    }

    void OnSetWeaponRotation(Action<Transform> func)
    {
        func?.Invoke(transform);
    }

    void OnChangeTextBulletStrategy(IShootingStrategy strategyType, string bulletStrategyType)
    {
        textMeshProWeapon.text = bulletStrategyType;
    }
    
    void OnAmountChanged(int currentAmmo, int maxAmmo)
    {
        textMeshProBullet.text = $"{currentAmmo}/{maxAmmo}";
    }
}