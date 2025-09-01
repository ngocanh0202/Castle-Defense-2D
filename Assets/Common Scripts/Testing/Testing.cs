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
        ObjectPooler.InitObjectPooler<PopupText>(
            KeyOfObjPooler.PopupText.ToString(),
            3,
            popupText,
            (textPopup) =>
            {
                textPopup.transform.SetParent(transformHolderText);
            }
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ConfirmModalSystem.Instance.ShowConfirmModal("Are you sure to quit game?", 2, OnListenerClickAccept: () =>
            {
                NotificationSystem.Instance.ShowNotification("YES........!!", NotificationType.Info);
            });
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            CreateGameObject.CreateCountdown(
                5, Vector3.zero, CountdownOptions.Seconds, transformParent: GameObject.Find(StringDefault.Canvas.ToString()).transform, onFinish: (textMeshPro) =>
                {
                    NotificationSystem.Instance.ShowNotification("Countdown Finished", NotificationType.Success);
                    Destroy(textMeshPro.gameObject);
                });
        }
        if (Input.GetKeyDown(KeyCode.B))
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