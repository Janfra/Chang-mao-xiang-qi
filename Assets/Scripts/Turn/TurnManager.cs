using System;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour, ITurnManager
{
    [SerializeField]
    private SO_TurnNotifier _turnNotifier;
    [SerializeField]
    private TurnHandler _turnState;

    [Header("Debug")]
    [SerializeField]
    private bool _debugStateChanges = false;

    public event UnityAction OnEnemyTurn { 
        add => _turnState.OnEnemyTurn += value;
        remove => _turnState.OnEnemyTurn -= value;
    }

    public event UnityAction OnPlayerTurn { 
        add => _turnState.OnPlayerTurn += value;
        remove => _turnState.OnPlayerTurn -= value;
    }

    public TurnState CurrentTurnState => _turnState.CurrentTurnState;

    private void Awake()
    {
        _turnState.Awake();
        _turnNotifier.AssignManager(this);

        _turnNotifier.TurnManager.OnEnemyTurn += () => {
            if (_debugStateChanges)
                Debug.Log("Enemy Turn Started");
        };
        _turnNotifier.TurnManager.OnPlayerTurn += () => {
            if (_debugStateChanges)
                Debug.Log("Player Turn Started");
        };
    }

    [ContextMenu("Request End Player Turn")]
    public void RequestEndPlayerTurn()
    {
        // Add some logic for waiting to end turn after actions are completed, if needed
        _turnState.TryEndPlayerTurn();
    }

    [ContextMenu("Request End Enemy Turn")]
    public void RequestEndEnemyTurn()
    {
        _turnState.TryEndEnemyTurn();
    }
}

[Serializable]
public class TurnHandler
{
    [SerializeField]
    private TurnState _initialState = TurnState.Player;

    public TurnState CurrentTurnState { get; private set; }
    public event UnityAction OnEnemyTurn;
    public event UnityAction OnPlayerTurn;

    public void Awake()
    {
        CurrentTurnState = _initialState;
    }

    [ContextMenu("Request End Player Turn")]
    public bool TryEndPlayerTurn()
    {
        if (CurrentTurnState != TurnState.Player)
        {
            Debug.LogWarning("Attempted to end player turn while it is not the player's turn.");
            return false;
        }

        CurrentTurnState = TurnState.Enemy;
        OnEnemyTurn?.Invoke();
        return true;
    }

    [ContextMenu("Request End Enemy Turn")]
    public bool TryEndEnemyTurn()
    {
        if (CurrentTurnState != TurnState.Enemy)
        {
            Debug.LogWarning("Attempted to end enemy turn while it is not the enemy's turn.");
            return false;
        }

        CurrentTurnState = TurnState.Player;
        OnPlayerTurn?.Invoke();
        return true;
    }
}

public enum TurnState
{
    Player,
    Enemy
}