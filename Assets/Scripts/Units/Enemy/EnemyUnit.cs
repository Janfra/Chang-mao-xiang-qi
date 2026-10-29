using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyUnit : Unit
{
    [SerializeField] private Vector2 KingPos;
    public event UnityAction OnEndIndividulTurn;

    [SerializeField] private Attack _attack;
    [SerializeField] private AnimationClip _deathClip;

    private GridMovement _gridMovement;

    private void OnDestroy()
    {
        _attack.OnDestroy();
    }

    protected virtual Vector2 GetBestMove()
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
        if (options.Length == 0)
        {
            return false;
        }
        else
        {
            _attack.DealDamageAt(options[Random.Range(0, options.Length)].Position);
            return true;
        }
    }

    protected void Awake()
    {
        _attack.Source = transform;
        _gridMovement = GetComponent<GridMovement>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDie()
    {
        EnemyManager.Instance.removeEnemy(this);
        Destroy(gameObject, _deathClip ? _deathClip.length + 0.01f : 0.0f);
    }

    public void Taketurn()
    {
        if (transform.position.y == 7 || !TryAttack())
        {
            _gridMovement.OnMoveCompleted += ClearAndEndTurn;
            _gridMovement.MoveTo(GetBestMove());
        }
        else
        {
            EndTurn();
        }
    }

    private void ClearAndEndTurn()
    {
        _gridMovement.OnMoveCompleted -= ClearAndEndTurn;
        EndTurn();
    }

    private void EndTurn()
    {
        OnEndIndividulTurn.Invoke();
    }
}
