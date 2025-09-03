using UnityEngine;

public abstract class Weapon : MonoBehaviour{
    [Header("Weapon Stat")]
    [SerializeField] public bool isAttacking = false;
    [SerializeField] protected Transform parentTransform;
    [SerializeField] protected Transform weaponTransform;
    [SerializeField] protected float radius;
    [SerializeField] protected float distance; 
    protected virtual void Awake()
    {
        parentTransform = transform.parent;
        weaponTransform = transform;
        radius = 3f;
    }
    public abstract void Attack(bool isPlayer = false);
    protected virtual void StopAttack()
    {
        isAttacking = false;
    }
}
