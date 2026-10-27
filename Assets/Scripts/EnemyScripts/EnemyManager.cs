using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    private List<GameObject> _enemies;
    [SerializeField] private SO_TurnNotifier SO_turn;
    [SerializeField] private SO_CollisionConstraint SO_ColConst;
    [SerializeField] private SO_GridSettings SO_GridSet;
    [SerializeField] private GameObject enemyPrefab;
    private int List_through;
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
        _enemies = new List<GameObject>();
    }


    private void OnDestroy()
    {
        SO_turn.TurnManager.OnEnemyTurn -= DoTurn;
    }


    private void CreateNewEnemy()
    {
        _enemies.Add(Instantiate(enemyPrefab, new Vector3(Random.Range(-4,4), 6, 0), Quaternion.identity));
        _enemies[_enemies.Count - 1].GetComponent<GridMovement>().OnMoveCompleted += MoveEach;
    }

    

    public void DoTurn()
    {

        Vector3 kingPos;
        kingPos = KingSingleton.Instance.GetKingPos();
        CreateNewEnemy();
        List_through = 0;
        _enemies[List_through].GetComponent<GridMovement>().MoveTo(_enemies[List_through].GetComponent<EnemyUnit>().GetBestMove());
        
        

        

        
    }

    private void MoveEach()
    {
        List_through++;

        if (List_through == _enemies.Count)
        {
            SO_turn.TurnManager.RequestEndEnemyTurn();
        }
        else
        {
            _enemies[List_through].GetComponent<GridMovement>().MoveTo(_enemies[List_through].GetComponent<EnemyUnit>().GetBestMove());
        }
        
    }

}
