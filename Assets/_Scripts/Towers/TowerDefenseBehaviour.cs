using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseBehaviour : MonoBehaviour
{
    [SerializeField] private float rangeToAttack = 4f;
    [SerializeField] bool canAttack;
    [SerializeField] Gun gun;
    [SerializeField] Transform gunTransform;
    [SerializeField] Transform target;
    [SerializeField] KeyGuns keyGunType = KeyGuns.None;

    void Start()
    {
        canAttack = false;
        gun = GetComponentInChildren<Gun>();
        gunTransform = gun.transform;
        target = null;
        if (keyGunType == KeyGuns.None)
        {
            Debug.LogWarning($"TowerDefenseBehaviour {gameObject.name}: KeyGunType is None, please set a valid KeyGunType.");
        }
        else
        {
            gun.SetStrategy(keyGunType);
        }
    }

    void Update()
    {
        CheckTargetInRange();
        HandleAttack();
    }

    void CheckTargetInRange()
    {
        if (IsEnemyInRange(out Collider2D enemyCollider))
        {
            target = enemyCollider.transform;
            canAttack = true;
        }
        else
        {
            target = null;
            canAttack = false;
        }
    }

    void HandleAttack()
    {
        if (canAttack && target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            gunTransform.right = direction;
            gun.Attack(false);
        }
    }

    bool IsEnemyInRange(out Collider2D enemyCollider)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, rangeToAttack);
        foreach (var collider in hitColliders)
        {
            IReceiveDamage enemyReceiveDamage = collider.GetComponent<IReceiveDamage>();
            if (enemyReceiveDamage != null && !collider.gameObject.CompareTag(gameObject.tag))
            {
                enemyCollider = collider;
                return true;
            }
        }
        enemyCollider = null;
        return false;
    }

    void OnDrawGizmos()
    {
        if (IsEnemyInRange(out Collider2D enemyCollider))
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(transform.position, rangeToAttack);
    }
}