public interface IReceiveDamage
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    float DamageInterval { get; }
    void ReceiveDamage(float damage);
    void Die();
}
