using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] protected SO_MovementType _movementType;
    [SerializeField] protected GridRestrictionComponent _gridRestrictionComponent;
    
    [field: SerializeField]
    public SO_MovementSettings MoveSettings { get; private set; } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Vector2[] GetPossiblePositions()
    {
        var positions = _movementType.GetMovePositionsAtSteps(transform.position, MoveSettings ? MoveSettings.MaxMoveSteps : 1);
        if (_gridRestrictionComponent)
        {
            positions = _gridRestrictionComponent.FilterValidPositions(positions);
        }
        return positions;
    }
}
