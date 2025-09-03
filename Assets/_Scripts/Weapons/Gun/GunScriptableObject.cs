using UnityEngine;

[System.Serializable]
public class BulletState
{
    [SerializeField] public float speed;
    [SerializeField] public float damage;
    [SerializeField] public float lifeTime;
}

[CreateAssetMenu(fileName = "Gun", menuName = "Gun/GunInfor", order = 1)]
public class GunScriptableObject : ScriptableObject
{
    public int maxAmmo;
    public int numberOfBullets;
    public float countDown;
    public float speedReloadAmmo;
    [SerializeField] public BulletState bulletState;
}
