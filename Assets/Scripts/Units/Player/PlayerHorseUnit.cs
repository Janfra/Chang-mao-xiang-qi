using UnityEngine;

public class PlayerHorseUnit : PlayerUnit
{
    [SerializeField]
    private Attack _attack;

    private bool _hasMoved => MoveCount > 0;
    private bool _hasAttacked = false;

    protected override void Awake()
    {
        base.Awake();
        _attack.Source = transform;
    }

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

    protected override void OnSelect()
    {
        if (!IsActive)
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
            HidePositionSelections();
            _attack.DealDamageAt(position);
        }
    }

    protected override void OnMoveComplete()
    {
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

        ShowPositionSelections(attackPositions);
    }
}
