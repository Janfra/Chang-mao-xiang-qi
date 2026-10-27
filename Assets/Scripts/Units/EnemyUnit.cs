using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyUnit : Unit
{
    [SerializeField] private Vector2 KingPos;

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
}
