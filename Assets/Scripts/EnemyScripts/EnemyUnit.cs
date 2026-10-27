using Unity.Mathematics;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private SO_MovementType _movementType;
    [SerializeField] private GridRestrictionComponent _gridRestrictionComponent;
    [SerializeField] private Vector2 KingPos;

    public Vector2 GetBestMove()
    {
        var positions = _movementType.GetMovePositionsAtSteps(transform.position, 1);
        if (_gridRestrictionComponent)
        {
            positions = _gridRestrictionComponent.FilterValidPositions(positions);
        }
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
