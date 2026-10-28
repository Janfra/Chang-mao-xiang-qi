using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [SerializeField] private SO_TurnNotifier SO_turn;
    [SerializeField] private SO_CollisionConstraint SO_ColConst;
    [SerializeField] private SO_GridSettings SO_GridSet;
    [SerializeField] private EnemyUnit _enemyPrefab;

    private List<EnemyUnit> _enemies = new();
    private Queue<EnemyUnit> _turnQueue = new();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
    }

    private void Start()
    {
        SO_turn.TurnManager.OnEnemyTurn += DoTurn;
    }


    private void OnDestroy()
    {
        SO_turn.TurnManager.OnEnemyTurn -= DoTurn;
    }


    private void CreateNewEnemy()
    {
        var enemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-4, 5), 7, 0), Quaternion.identity);
        enemy.OnEndIndividulTurn += TakeEachTurn;
        _enemies.Add(enemy);
    }

    public void removeEnemy(EnemyUnit toRemove)
    {
        toRemove.OnEndIndividulTurn -= TakeEachTurn;
        _enemies.Remove(toRemove);
    }
    

    public void DoTurn()
    {

        



        CreateNewEnemy();
        CreateTurnQueue();
        TakeEachTurn();












    }

    private void CreateTurnQueue()
    {
        _turnQueue.Clear();
        foreach (var enemy in _enemies)
        {
            _turnQueue.Enqueue(enemy);
        }
    }

    private void TakeEachTurn()
    {
        if (_turnQueue.Count == 0)
        {
            SO_turn.TurnManager.RequestEndEnemyTurn();
        }
        else
        {
            var enemy = _turnQueue.Dequeue();
            enemy.Taketurn();
        }
    }

}
