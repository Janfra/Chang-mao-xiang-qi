using UnityEngine;

public class GridRestrictionComponent : MonoBehaviour
{
    public SO_GridSettings GridSettings;
    public SO_GridConstraint[] Constraints;

    private void Awake()
    {
        if (GridSettings == null)
        {
            Debug.LogError("GridSettings is not assigned in GridRestriction.");
        }
    }

    public Vector3[] FilterValidPositions(Vector3[] positions)
    {
        foreach (var constraint in Constraints)
        {
            positions = System.Array.FindAll(positions, pos => constraint.IsPositionValid(transform, pos, GridSettings));
        }
        return positions;
    }

    public Vector2[] FilterValidPositions(Vector2[] positions)
    {
        foreach (var constraint in Constraints)
        {
            positions = System.Array.FindAll(positions, pos => constraint.IsPositionValid(transform, pos, GridSettings));
        }
        return positions;
    }
}
