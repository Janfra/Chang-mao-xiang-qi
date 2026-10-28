using UnityEngine;

public class PlayerHorseUnit : PlayerUnit
{
    [SerializeField]
    private Attack _attack;

    [SerializeField]
    private SO_Token _attackToken;

    private bool _hasMoved => MoveCount > 0;
    private bool _hasAttacked = false;
    private bool _canRevive = false;

    protected override void Awake()
    {
        base.Awake();
        _attack.Source = transform;
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
            ShowAttackOptions();
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
            base.OnPositionSelect(position);
        }
        else
        {
            _hasAttacked = true;
            HidePositionSelections();
            _attack.DealDamageAt(position);
        }
    }

    protected override void OnMoveComplete()
    {
        base.OnMoveComplete();
        ShowAttackOptions();
    }

    private void ShowAttackOptions()
    {
        var options = _attack.GetValidAttackOptions();
        Vector2[] attackPositions = new Vector2[options.Length];
        for (int i = 0; i < attackPositions.Length; i++)
        {
            attackPositions[i] = options[i].Position;
        }

        ShowPositionSelections(attackPositions, _attackToken);
    }
}
