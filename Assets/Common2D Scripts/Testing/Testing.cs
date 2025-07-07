using TMPro;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] TextMeshProUGUI textMeshProWeapon;
    [SerializeField] TextMeshProUGUI textMeshProBullet;
    void Start()
    {
        gun.OnSetBulletStrategy += OnChangeTextBulletStrategy;
        gun.OnAmmoChanged += OnAmountChanged;
        textMeshProWeapon.text = KeyOfObjPooler.BaseBullet.ToString();

        textMeshProBullet.text = "10/10";
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