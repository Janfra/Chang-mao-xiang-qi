using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerUnit : Unit
{
    [SerializeField]
    private SO_TurnNotifier _turnNotifier;
    [SerializeField]
    private SelectableComponent _selectableComponent;
    [SerializeField]
    private GridMovement _gridMovement;
    [SerializeField]
    private PositionSelector _positionSelector;

    [Header("Debug")]
    [SerializeField]
    private bool _drawDebugOnlySelected;

    public bool IsActive => _turnNotifier?.TurnManager?.CurrentTurnState == TurnState.Player;
    private bool _hasMoved = false;

    private void Awake()
    {
        _positionSelector.Awake(OnPositionSelect);
    }

    private void OnEnable()
    {
        if (_turnNotifier == null)
        {
            Debug.LogError("Turn Notifier is not assigned in PlayerUnit.");
            return;
        }

        ListenForTurnUpdates();
        _gridMovement.OnMoveCompleted += OnMoveComplete;
        _selectableComponent.OnSelected += OnSelect;
        _selectableComponent.OnDeselected += OnDeselect;
    }

    private void OnDisable()
    {
        StopListeningForTurnUpdates();
        _gridMovement.OnMoveCompleted -= OnMoveComplete;
        _selectableComponent.OnSelected -= OnSelect;
        _selectableComponent.OnDeselected += OnDeselect;
    }

    public void TryShowMoveOptions()
    {
        if (_hasMoved)
        {
            return;
        }

        Vector2[] positions = _movementType.GetMovePositionsAtSteps(transform.position, 1);
        positions = _gridRestrictionComponent.FilterValidPositions(positions);
        _positionSelector.ShowOptionsAt(positions);
    }

    private void ListenForTurnUpdates()
    {
        if (_turnNotifier.TurnManager != null)
        {
            _turnNotifier.TurnManager.OnPlayerTurn += OnTurnStart;
        }
    }

    private void StopListeningForTurnUpdates()
    {
        if (_turnNotifier.TurnManager != null)
        {
            _turnNotifier.TurnManager.OnPlayerTurn -= OnTurnStart;
        }
    }

    private void OnTurnStart()
    {
        _hasMoved = false;
        if (_selectableComponent.IsSelected)
        {
            TryShowMoveOptions();
        }
    }

    private void OnSelect()
    {
        if (!IsActive)
        {
            return;
        }

        TryShowMoveOptions();
    }

    private void OnDeselect()
    {
        if (!IsActive)
        {
            return;
        }

        _positionSelector.HideAll();
    }

    private void OnPositionSelect(Vector2 position)
    {
        if (_gridMovement.IsMoving || _hasMoved)
        {
            return;
        }

        _hasMoved = true;
        _gridMovement.MoveTo(position);
        _positionSelector.HideAll();
    }

    private void OnMoveComplete()
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
