using Unity.Mathematics;
using UnityEngine;

public class EnemyUnit : Unit
{
    
    [SerializeField] private Vector2 KingPos;


    public void Awake()
    {
        KingPos = new Vector2(0, -2);
    }
    public Vector2 GetBestMove()
    {
        var positions =  GetPossiblePosisions();
        Vector2 BestPos;
        BestPos = new Vector2();
        foreach (var position in positions)
        {
            if(BestPos == null || math.distance(position,KingPos) < math.distance(BestPos, KingPos))
            {
                BestPos = position;
            }
        }
        return BestPos;

    }
}
