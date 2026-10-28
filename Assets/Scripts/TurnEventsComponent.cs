using System;
using UnityEngine;
using UnityEngine.Events;

public class TurnEventsComponent : MonoBehaviour
{
    [SerializeField]
    private SO_TurnNotifier _turnNotifier;

    [SerializeField]
    private UnityEvent _onPlayerTurn;
    [SerializeField]
    private UnityEvent _onEnemyTurn;

    private void OnEnable()
    {
        if (_turnNotifier.TurnManager != null)
        {
            _turnNotifier.TurnManager.OnPlayerTurn += _onPlayerTurn.Invoke;
            _turnNotifier.TurnManager.OnEnemyTurn += _onEnemyTurn.Invoke;
        }
    }

    private void OnDisable()
    {
        if (_turnNotifier.TurnManager != null)
        {
            _turnNotifier.TurnManager.OnPlayerTurn -= _onPlayerTurn.Invoke;
            _turnNotifier.TurnManager.OnEnemyTurn -= _onEnemyTurn.Invoke;
        }
    }
}
