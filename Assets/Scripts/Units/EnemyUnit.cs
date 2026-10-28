using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyUnit : Unit
{
    [SerializeField] private Vector2 KingPos;
    public event UnityAction OnEndIndividulTurn;

    [SerializeField] private Attack _attack;

    public Vector2 GetBestMove()
    {
        var positions = GetPossiblePositions();
        KingPos = KingSingleton.Instance.GetKingPos();
        Vector2 BestPos;
        BestPos = new Vector2(0,9999);
        foreach (var position in positions)
        {
            if( math.distance(position,KingPos) < math.distance(BestPos, KingPos))
            {
                BestPos = position;
            }
            else if (math.distance(position, KingPos) < ( math.distance(BestPos, KingPos) + Random.Range(0f,0.7f) ))
            {
                BestPos = position;
            }
        }
        if (BestPos == new Vector2(0, 9999)) return transform.position;
        else return BestPos;

    }

    private bool TryAttack()
    {
        var options = _attack.GetValidAttackOptions();
        if (options.Length == 0 ) return false;
        else
        {
            Debug.Log("attack");
            _attack.DealDamageAt(options[Random.Range(0,options.Length)].Position);
            endturn();
            return true;
        }
        
    }

    protected virtual void Awake()
    {
        _attack.Source = transform;
    }

    public void Start()
    {
        
        GetComponent<GridMovement>().OnMoveCompleted += endturn;

    }

    public void Taketurn()
    {

        if (TryAttack() == false){ GetComponent<GridMovement>().MoveTo(GetBestMove()); }


    }


    private void endturn()
    {
        OnEndIndividulTurn.Invoke();
    }
}
