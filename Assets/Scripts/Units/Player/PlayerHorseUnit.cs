using System.Linq;
using UnityEngine;

public class PlayerHorseUnit : PlayerUnit
{
    [Header("Configuration")]
    [SerializeField]
    private Attack _attack;
    [SerializeField]
    private SO_Token _attackToken;

    [Header("Shared Information")]
    [SerializeField]
    private SO_PlayerActionDescription _attackActionDescription;

    private bool _hasMoved => MoveCount > 0;
    private bool _hasAttacked = false;
    private bool _canRevive = false;

    protected override void Awake()
    {
        base.Awake();
        _attack.Source = transform;
        _attack.OnCacheTargetLost += UpdateActiveAction;
    }

    private void OnDestroy()
    {
        _attack.OnDestroy();
    }

    public override void TryShowMoveOptions()
    {
        if (_hasMoved && _hasAttacked || IsDead)
        {
            return;
        }

        base.TryShowMoveOptions();
    }

    protected override void OnTurnStart()
    {
        if (_canRevive)
        {
            HealthComponent.Health.Reset(); // Revive the horse after losing a turn being dead
            _canRevive = false;
        }

        base.OnTurnStart();
        _hasAttacked = false;
    }

    protected override void OnTurnEnd()
    {
        base.OnTurnEnd();

        _attack.ClearCacheValidAttackResults();

        if (IsDead)
        {
            _canRevive = true;
        }
    }

    protected override void OnSelect()
    {
        if (!IsActive || IsDead)
        {
            return;
        }

        if (!_hasMoved)
        {
            TryShowMoveOptions();
        }
        else if (!_hasAttacked)
        {
            ShowAttackOptions(_attack.CachedAttackResults.ToArray());
        }
    }

    protected override void OnPositionSelect(Vector2 position)
    {
        if (_hasMoved && _hasAttacked || !IsActive || IsDead)
        {
            HidePositionSelections();
            return;
        }

        if (!_hasMoved)
        {
            if (TryMoveTo(position))
            {
                if (_attack.HasValidAttackOptionsAt(position))
                {
                    AvailablePlayerAction = _attackActionDescription;
                }
                else
                {
                    AvailablePlayerAction = null;
                }
            }
        }
        else
        {
            AvailablePlayerAction = null;
            _hasAttacked = true;
            HidePositionSelections();
            _attack.DealDamageAt(position);
        }
    }

    protected override void OnMoveComplete()
    {
        base.OnMoveComplete();
        var options = _attack.GetValidAttackOptions();
        ShowAttackOptions(options);
    }

    private void ShowAttackOptions(AttackResult[] attackOptions)
    {
        if (attackOptions == null)
        {
            return;
        }

        Vector2[] attackPositions = new Vector2[attackOptions.Length];
        for (int i = 0; i < attackPositions.Length; i++)
        {
            attackPositions[i] = attackOptions[i].Position;
        }

        ShowPositionSelections(attackPositions, _attackToken);
    }

    private void UpdateActiveAction()
    {
        if (AvailablePlayerAction == _attackActionDescription)
        {
            if (_attack.CachedAttackResults.Count == 0)
            {
                AvailablePlayerAction = null;
            }
        }
    }
}
