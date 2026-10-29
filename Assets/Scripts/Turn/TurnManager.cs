using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour, ITurnManager
{
    [SerializeField]
    private SO_TurnNotifier _turnNotifier;
    [SerializeField]
    private TurnHandler _turnState;
    [SerializeField]
    private SO_PlayerUnitRegistry _playerUnitRegistry;
    private bool _isQueuedForEndingTurn;
    private int turnNumb = 1;

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


    public int GetTurnNumb()
    {
        return turnNumb;
    }


    [ContextMenu("Request End Player Turn")]
    public void RequestEndPlayerTurn()
    {
        if (_isQueuedForEndingTurn || KingSingleton.Instance && KingSingleton.Instance.IsKingDead)
        {
            return;
        }

        if (!ArePlayerUnitsReadyForTurnEnd())
        {
            StartCoroutine(TryEndTurnAfterUnitsAreReady());
        }
        else
        {
            _turnState.TryEndPlayerTurn();
        }
    }

    [ContextMenu("Request End Enemy Turn")]
    public void RequestEndEnemyTurn()
    {
        turnNumb++;
        _turnState.TryEndEnemyTurn();
    }

    private bool ArePlayerUnitsReadyForTurnEnd()
    {
        foreach (var unit in _playerUnitRegistry.PlayerUnits)
        {
            if (!unit.IsReadyForTurnEnd)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator TryEndTurnAfterUnitsAreReady()
    {
        _isQueuedForEndingTurn = true;
        yield return new WaitUntil(ArePlayerUnitsReadyForTurnEnd);
        _isQueuedForEndingTurn = false;
        _turnState.TryEndPlayerTurn();
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