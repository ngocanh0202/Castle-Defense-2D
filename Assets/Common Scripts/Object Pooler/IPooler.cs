using System;

public interface IPoolObject
{
    string PoolName { get; set; }
    Action<object, string> OnSetInactive { get; set; }
}
