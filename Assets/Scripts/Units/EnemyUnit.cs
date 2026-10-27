using Unity.Mathematics;
using UnityEngine;

public class EnemyUnit : Unit
{
    [SerializeField] private Vector2 KingPos;

    public Vector2 GetBestMove()
    {
        var positions = GetPossiblePositions();
        
        Vector2 BestPos;
        BestPos = new Vector2(0,9999);
        foreach (var position in positions)
        {
            if( math.distance(position,KingPos) < math.distance(BestPos, KingPos))
            {
                BestPos = position;
            }
        }
        if (BestPos == new Vector2(0, 9999)) return transform.position;
        else return BestPos;

    }
}
