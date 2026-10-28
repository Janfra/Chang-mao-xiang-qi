using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SelectableComponent))]
public class PlayerUnit : Unit
{
    public event UnityAction OnReadyForTurnEnd;
    public event UnityAction<PlayerUnit> OnAvailableActionUpdated;

    [SerializeField]
    private SO_TurnNotifier _turnNotifier;

    [field: SerializeField]
    public SelectableComponent SelectableComponent { get; private set; }

    [field: SerializeField]
    public SO_PlayerActionDescription MoveActionDescription { get; private set; }

    [SerializeField]
    private GridMovement _gridMovement;

    [SerializeField]
    private PositionSelector _positionSelector;

    [SerializeField]
    private SO_PlayerUnitRegistry _unitRegistry;


    [Header("Debug")]
    [SerializeField]
    private bool _drawDebugOnlySelected = true;

    public int MoveCount { get; private set; }
    public SO_PlayerActionDescription AvailablePlayerAction { get => _availablePlayerAction; protected set => UpdateAvailablePlayerAction(value); }
    public bool IsActive => _turnNotifier?.TurnManager?.CurrentTurnState == TurnState.Player;
    public bool IsReadyForTurnEnd { 
        get => _isReadyForTurnEnd; 
        protected set => SetReadyForTurnEnd(value); 
    }

    private bool _isReadyForTurnEnd = true;
    private SO_PlayerActionDescription _availablePlayerAction;

    protected virtual void Awake()
    {
        _positionSelector.Awake(OnPositionSelect);
        AvailablePlayerAction = MoveActionDescription;
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
        if (!IsActive || IsDead)
        {
            return;
        }

        Vector2[] positions = GetPossiblePositions();
        _positionSelector.ShowOptionsAt(positions);
    }

    protected override void OnDie()
    {
        // For player there is no need for special die logic yet other than blocking moves on dead
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
        HidePositionSelections();

        if (IsDead)
        {
            AvailablePlayerAction = null;
            return;
        }

        MoveCount = 0;
        AvailablePlayerAction = MoveActionDescription;
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
        if (!IsActive || IsDead)
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

    protected void ShowPositionSelections(Vector2[] positions, SO_Token token = null)
    {
        _positionSelector.HideAll();
        _positionSelector.ShowOptionsAt(positions, token);
    }

    protected void HidePositionSelections()
    {
        _positionSelector.HideAll();
    }

    protected bool TryMoveTo(Vector2 position)
    {
        if (_gridMovement.IsMoving)
        {
            return false;
        }

        MoveCount++;
        IsReadyForTurnEnd = false;
        _gridMovement.MoveTo(position);
        _positionSelector.HideAll();
        return true;
    }

    protected virtual void OnPositionSelect(Vector2 position)
    {
        if (MoveCount > 0)
        {
            return;
        }

        if (TryMoveTo(position))
        {
            AvailablePlayerAction = null;
        }
    }

    protected virtual void OnMoveComplete()
    {
        IsReadyForTurnEnd = true;
    }

    private void UpdateAvailablePlayerAction(SO_PlayerActionDescription description)
    {
        if (_availablePlayerAction == description)
        {
            return;
        }

        _availablePlayerAction = description;
        OnAvailableActionUpdated?.Invoke(this);
    }

    private void SetReadyForTurnEnd(bool isReady)
    {
        _isReadyForTurnEnd = isReady;
        if (_isReadyForTurnEnd)
        {
            OnReadyForTurnEnd?.Invoke();
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
