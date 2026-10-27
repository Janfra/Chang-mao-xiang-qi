using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Cardinal Movement", menuName = "ScriptableObjects/Movement Types/Cardinal")]
public class SO_CardinalMovement : SO_MovementType
{
    private HashSet<Vector2> _positions = new();

    public override Vector2[] GetMovePositionsAtSteps(Vector2 origin, int steps)
    {
        // For now only ever do 1 step max for simplicity
        _positions.Clear();
        steps = Mathf.Clamp(steps, 1, GridExtensions.GridLength); // Clamp it to be within max grid size
        const int DirectionCount = (int)Direction.UpLeft; // Cardinal directions go up to UpLeft, so 4
        int size = steps * DirectionCount;
        _positions.EnsureCapacity(size);

        for (int i = 0; i < DirectionCount; i++)
        {
            for (int j = 1; j <= steps; j++)
            {
                Direction direction = (Direction)i;
                _positions.Add(origin + direction.ToVector2() * (GridSettings.GridSize * j));
            }
        }

        return _positions.ToArray();
    }
}
