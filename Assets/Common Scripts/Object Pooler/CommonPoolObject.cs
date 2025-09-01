using System;
using UnityEngine;

public class CommonPoolObject : MonoBehaviour, IPoolObject
{
    public Action<object, string> OnSetInactive { get; set; }
    public virtual string PoolName { get; set; }

    protected virtual void OnDisable()
    {
        OnSetInactive?.Invoke(this, PoolName);
    }
}
