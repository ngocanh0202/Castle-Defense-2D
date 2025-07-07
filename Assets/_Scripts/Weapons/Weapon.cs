using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Status")]
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected Transform parentTransform;
    [SerializeField] protected Transform weaponTransform;
    [SerializeField] protected float radius;
    [SerializeField] protected float distance; 
    protected virtual void Start()
    {
        parentTransform = transform.parent;
        weaponTransform = transform;
        radius = 3f;
    }
    protected virtual void Update()
    {
        // Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Vector2 direction = mouseWorldPos - transform.position;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0f, 0f, angle);


        distance = Vector2.Distance(transform.parent.position, weaponTransform.position);
        if(distance >  radius){
            Vector2 directionToCenter = (transform.parent.position - weaponTransform.position).normalized;
            Vector2 center = new Vector2(transform.parent.position.x, transform.parent.position.y);
            weaponTransform.position = center - directionToCenter * radius;
        }
    }
    public abstract void Attack();
    protected virtual void StopAttack()
    {
        isAttacking = false;
    }
}
