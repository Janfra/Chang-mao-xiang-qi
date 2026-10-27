using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerUnit : MonoBehaviour
{
    [SerializeField]
    private SO_TurnNotifier _turnNotifier;
    [SerializeField]
    private SelectableComponent _selectableComponent;
    [SerializeField]
    private GridRestrictionComponent _gridRestrictionComponent;
    [SerializeField]
    private SO_MovementType _movementType;

    [Header("Debug")]
    [SerializeField]
    private bool _drawDebugOnlySelected;

    private bool _isActive => _turnNotifier?.TurnManager?.CurrentTurnState == TurnState.Player;

    private void OnEnable()
    {
        if (_turnNotifier == null)
        {
            Debug.LogError("Turn Notifier is not assigned in PlayerUnit.");
            return;
        }

        ListenForTurnUpdates();
    }

    private void OnDisable()
    {
        StopListeningForTurnUpdates();
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
        if (_selectableComponent.IsSelected)
        {

        }
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
            var positions = _movementType.GetMovePositionsAtSteps(transform.position, 1);
            if (_gridRestrictionComponent)
            {
                positions = _gridRestrictionComponent.FilterValidPositions(positions);
            }

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
