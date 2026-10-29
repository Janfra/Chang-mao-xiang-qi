using UnityEngine.Events;

public interface ITurnManager
{
    TurnState CurrentTurnState { get; }
    event UnityAction OnEnemyTurn;
    event UnityAction OnPlayerTurn;
    void RequestEndPlayerTurn();
    void RequestEndEnemyTurn();

    int GetTurnNumb();
}
