using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField]
    private SO_TurnNotifier _turnNotifier;
    [SerializeField]
    private SelectableComponent _selectableComponent;
    [SerializeField]
    private GridRestrictionComponent _gridRestrictionComponent;

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
}
