using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private SO_TurnNotifier SO_turn;
    [SerializeField] private SO_CollisionConstraint SO_ColConst;
    [SerializeField] private SO_GridSettings SO_GridSet;
    private void Awake()
    {
        SO_turn.TurnManager.OnEnemyTurn += DoTurn;
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
    private void OnDestroy()
    {
        SO_turn.TurnManager.OnEnemyTurn -= DoTurn;
    }

    public void AddEnemy()
    {}

    public void DoTurn()
    {
        Vector3 kingPos;
        kingPos = KingSingleton.Instance.GetKingPos();
        
        foreach (var item in _enemies)
        {

            item.GetComponent<GridMovement>().MoveTo(item.GetComponent<EnemyUnit>().GetBestMove());

        }
        


        SO_turn.TurnManager.RequestEndEnemyTurn();
    }

}
