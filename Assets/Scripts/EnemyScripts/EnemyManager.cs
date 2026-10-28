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
        _enemies.Add(Instantiate(enemyPrefab, new Vector3(Random.Range(-4,5), 7, 0), Quaternion.identity));
        _enemies[_enemies.Count - 1].GetComponent<EnemyUnit>().OnEndIndividulTurn += TakeEachTurn;
    }

    public void removeEnemy(GameObject toRemove)
    {
        _enemies.Remove(toRemove);
        _enemies[_enemies.Count - 1].GetComponent<EnemyUnit>().OnEndIndividulTurn -= TakeEachTurn;
    }
    

    public void DoTurn()
    {

        
        CreateNewEnemy();
        List_through = 0;
        TakeEachTurn();






    }

    private void TakeEachTurn()
    {
        

        if (List_through == _enemies.Count )
        {
            SO_turn.TurnManager.RequestEndEnemyTurn();
        }
        else
        {
            _enemies[List_through].GetComponent<EnemyUnit>().Taketurn();
        }
        List_through++;
    }

}
