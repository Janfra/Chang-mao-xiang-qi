using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Attack
{
    public event UnityAction OnCacheTargetLost;

    [HideInInspector]
    public Transform Source;
    [SerializeField]
    private UnityEvent _onAttacked;

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

    private HashSet<AttackResult> _cacheValidAttackResults = new();

    public IReadOnlyCollection<AttackResult> CachedAttackResults => _cacheValidAttackResults;

    public void OnDestroy()
    {
        ClearCacheValidAttackResults();
    }

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
        _onAttacked?.Invoke();
    }

    public Vector2[] GetAttackDirections()
    {
        return _attackMove.GetMovePositionsAtSteps(Source.position, _attackRange);
    }

    public Vector2[] GetAttackDirectionsFrom(Vector2 location)
    {
        return _attackMove.GetMovePositionsAtSteps(location, _attackRange);
    }

    public AttackResult[] GetValidAttackOptions()
    {
        ClearCacheValidAttackResults();
        var positions = GetAttackDirections();
        foreach (var position in positions)
        {
            if (TryGetAttackResultAt(position, out AttackResult result))
            {
                result.HitHealth.Health.OnDied += UpdateCacheAttackResults;
                _cacheValidAttackResults.Add(result);
            }
        }

        return _cacheValidAttackResults.ToArray();
    }

    public void ClearCacheValidAttackResults()
    {
        foreach (var result in _cacheValidAttackResults)
        {
            result.HitHealth.Health.OnDied -= UpdateCacheAttackResults;
        }
        _cacheValidAttackResults.Clear();
    }

    private void UpdateCacheAttackResults()
    {
        _cacheValidAttackResults.RemoveWhere(result => result.HitHealth.Health.IsDead);
        OnCacheTargetLost?.Invoke();
    }

    public bool HasValidAttackOptionsAt(Vector2 position)
    {
        var positions = GetAttackDirectionsFrom(position);
        foreach(var target in positions)
        {
            if (GetAttackHitColliderAt(target) != null)
            {
                return true;
            }
        }

        return false;
    }

    public bool TryGetAttackResultAt(Vector2 position, out AttackResult result)
    {
        var collider = GetAttackHitColliderAt(position);
        if (collider)
        {
            if (collider.TryGetComponent(out HealthComponent health) && !health.Health.IsDead)
            {
                result = new AttackResult(position, health);
                return true;
            }
        }

        result = null;
        return false;
    }

    private Collider2D GetAttackHitColliderAt(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, _attackRadius, _attackMask.value);
    }
}
