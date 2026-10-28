using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Attack
{
    [HideInInspector]
    public Transform Source;

    [SerializeField]
    private SO_MovementType _attackMove;
    [SerializeField]
    private int _attackRange = 1;
    [SerializeField]
    private float _attackRadius = 0.4f;
    [SerializeField]
    private LayerMask _attackMask;
    [SerializeField]
    private int _damage = 1;

    public HashSet<AttackResult> _validAttackPoints { get; private set; } = new();

    public void DealDamageAt(Vector2 position)
    {
        if (TryGetAttackResultAt(position, out var attackResult))
        {
            DealDamageTo(attackResult.HitHealth);
        }
    }

    public void DealDamageTo(HealthComponent healthComponent)
    {
        healthComponent.Health.TakeDamage(_damage);
    }

    public Vector2[] GetAttackDirections()
    {
        return _attackMove.GetMovePositionsAtSteps(Source.position, _attackRange);
    }

    public AttackResult[] GetValidAttackOptions()
    {
        _validAttackPoints.Clear();
        var positions = GetAttackDirections();
        foreach (var position in positions)
        {
            if (TryGetAttackResultAt(position, out AttackResult result))
            {
                _validAttackPoints.Add(result);
            }
        }

        return _validAttackPoints.ToArray();
    }

    public bool TryGetAttackResultAt(Vector2 position, out AttackResult result)
    {
        var collider = Physics2D.OverlapCircle(position, _attackRadius, _attackMask.value);
        if (collider)
        {
            if (collider.TryGetComponent(out HealthComponent health))
            {
                result = new AttackResult(position, health);
                return true;
            }
        }

        result = null;
        return false;
    }
}
