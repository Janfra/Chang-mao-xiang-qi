using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onDied;
    [SerializeField]
    private UnityEvent _onTookDamage;

    [SerializeField]
    private SO_Health healthConfiguration;

    public Health Health { get; private set; }

    private void Awake()
    {
        Health = new(healthConfiguration ? healthConfiguration.MaxHealth : 1);
    }

    private void OnEnable()
    {
        Health.OnDied += _onDied.Invoke;
        Health.OnDamaged += _onTookDamage.Invoke;
    }

    private void OnDisable()
    {
        Health.OnDied -= _onDied.Invoke;
        Health.OnDamaged += _onTookDamage.Invoke;
    }
}

[Serializable]
public class Health
{
    public event UnityAction OnDied;
    public event UnityAction OnDamaged;
    public event UnityAction OnHealed;

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public Health(int maxHealth = 1)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Don't need to dmg the dead
        if (IsDead)
        {
            return;
        }

        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            OnDied?.Invoke();
            CurrentHealth = 0;
        }
        else
        {
            OnDamaged?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        // Cant heal the dead
        if (IsDead)
        {
            return;
        }

        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        OnHealed?.Invoke(); 
    }

    public void Reset()
    {
        CurrentHealth = MaxHealth;
    }
}