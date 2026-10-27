using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack
{
    public Transform Source;

    [SerializeField]
    private SO_MovementType _attackMove;
    [SerializeField]
    private int _attackRange = 1;
    [SerializeField]
    private float _attackRadius = 0.4f;
    [SerializeField]
    private LayerMask _attackMask;

    private HashSet<Vector2> _validAttackPoints = new();

    public Vector2[] GetAttackDirections()
    {
        return _attackMove.GetMovePositionsAtSteps(Source.position, _attackRange);
    }

    public Vector2[] GetValidAttackOptions()
    {
        var positions = GetAttackDirections();
        foreach (var position in positions)
        {
            var result = Physics2D.OverlapCircle(position, _attackRadius, _attackMask.value);
            if (result)
            {
                var health = result.GetComponent<HealthComponent>();
                
            }
        }

        return positions;
    }
}

public class PlayerHorseUnit : PlayerUnit
{
    [SerializeField]
    private Attack _attack;

    private bool _hasMoved => MoveCount > 0;
    private bool _hasAttacked = false;

    public override void TryShowMoveOptions()
    {
        if (_hasMoved && _hasAttacked)
        {
            return;
        }

        base.TryShowMoveOptions();
    }

    protected override void OnTurnStart()
    {
        _hasAttacked = false;
        base.OnTurnStart();
    }

    protected override void OnPositionSelect(Vector2 position)
    {
        if (_hasMoved && _hasAttacked)
        {
            return;
        }

        if (!_hasMoved)
        {
            base.OnPositionSelect(position);
        }
        else
        {
            _hasAttacked = true;
        }
    }

    protected override void OnMoveComplete()
    {
        //var positions = _attack.GetAttackDirections();
    }
}
