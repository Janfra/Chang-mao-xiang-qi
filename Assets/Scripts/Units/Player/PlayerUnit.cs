using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SelectableComponent))]
public class PlayerUnit : Unit
{
    [SerializeField]
    private SO_TurnNotifier _turnNotifier;

    [field: SerializeField]
    public SelectableComponent SelectableComponent { get; private set; }

    [SerializeField]
    private GridMovement _gridMovement;

    [SerializeField]
    private PositionSelector _positionSelector;

    [SerializeField]
    private SO_PlayerUnitRegistry _unitRegistry;

    [Header("Debug")]
    [SerializeField]
    private bool _drawDebugOnlySelected = true;

    public bool IsActive => _turnNotifier?.TurnManager?.CurrentTurnState == TurnState.Player;
    public int MoveCount { get; private set; }

    protected virtual void Awake()
    {
        _positionSelector.Awake(OnPositionSelect);
    }

    protected virtual void OnEnable()
    {
        if (_turnNotifier == null)
        {
            Debug.LogError("Turn Notifier is not assigned in PlayerUnit.");
            return;
        }

        ListenForTurnUpdates();
        _gridMovement.OnMoveCompleted += OnMoveComplete;
        SelectableComponent.OnSelected += OnSelect;
        SelectableComponent.OnDeselected += OnDeselect;
        _unitRegistry?.Register(this);
    }

    protected virtual void OnDisable()
    {
        StopListeningForTurnUpdates();
        _gridMovement.OnMoveCompleted -= OnMoveComplete;
        SelectableComponent.OnSelected -= OnSelect;
        SelectableComponent.OnDeselected += OnDeselect;
        _unitRegistry?.Remove(this);
    }

    public virtual void TryShowMoveOptions()
    {
        if (!IsActive)
        {
            return;
        }

        Vector2[] positions = GetPossiblePositions();
        _positionSelector.ShowOptionsAt(positions);
    }

    private void ListenForTurnUpdates()
    {
        if (_turnNotifier.TurnManager != null)
        {
            _turnNotifier.TurnManager.OnPlayerTurn += OnTurnStart;
            _turnNotifier.TurnManager.OnEnemyTurn += OnTurnEnd;
        }
    }

    private void StopListeningForTurnUpdates()
    {
        if (_turnNotifier.TurnManager != null)
        {
            _turnNotifier.TurnManager.OnPlayerTurn -= OnTurnStart;
            _turnNotifier.TurnManager.OnEnemyTurn -= OnTurnEnd;

        }
    }

    protected virtual void OnTurnStart()
    {
        MoveCount = 0;
        if (SelectableComponent.IsSelected)
        {
            TryShowMoveOptions();
        }
    }

    protected virtual void OnTurnEnd()
    {
        HidePositionSelections();
    }

    protected virtual void OnSelect()
    {
        if (!IsActive)
        {
            return;
        }

        TryShowMoveOptions();
    }

    protected virtual void OnDeselect()
    {
        if (!IsActive)
        {
            return;
        }

        _positionSelector.HideAll();
    }

    protected void ShowPositionSelections(Vector2[] positions)
    {
        _positionSelector.HideAll();
        _positionSelector.ShowOptionsAt(positions);
    }

    protected void HidePositionSelections()
    {
        _positionSelector.HideAll();
    }

    protected virtual void OnPositionSelect(Vector2 position)
    {
        if (_gridMovement.IsMoving || MoveCount > 0)
        {
            return;
        }

        _gridMovement.MoveTo(position);
        MoveCount++;
        _positionSelector.HideAll();
    }

    protected virtual void OnMoveComplete()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!_drawDebugOnlySelected)
        {
            DrawUnitValidMovePositions();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_drawDebugOnlySelected)
        {
            DrawUnitValidMovePositions();
        }
    }

    private void DrawUnitValidMovePositions()
    {
        if (_movementType)
        {
            var positions = GetPossiblePositions();

            foreach (var position in positions)
            {
                Gizmos.DrawSphere(position, 0.5f);

#if UNITY_EDITOR
                Handles.Label(position, position.ToString());
#endif
            }
        }
    }
}
