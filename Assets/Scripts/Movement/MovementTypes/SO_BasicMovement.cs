using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Movement", menuName = "ScriptableObjects/Movement Types/Basic")]
public class SO_BasicMovement : SO_MovementType
{
    private HashSet<Vector2> _positions = new();

    public override Vector2[] GetMovePositionsAtSteps(Vector2 origin, int steps)
    {
        // For now only ever do 1 step max for simplicity
        _positions.Clear();
        steps = Mathf.Clamp(steps, 1, 1); // Clamp it to be within max grid size `GridExtensions.GridLength` when ready
        const int DirectionCount = (int)Direction.DIRECTION_COUNT;
        int size = steps * DirectionCount;
        _positions.EnsureCapacity(size);

        for (int i = 0; i < DirectionCount; i++)
        {
            Direction direction = (Direction)i;
            _positions.Add(origin + direction.ToVector2() * (GridSettings.GridSize * steps));
        }

        return _positions.ToArray();
    }
}
