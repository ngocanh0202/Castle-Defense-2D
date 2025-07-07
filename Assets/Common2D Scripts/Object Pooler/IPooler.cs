using System;

public interface IPooler
{
    event EventHandler<PoolerEventArgs> OnSetInactive;
}
