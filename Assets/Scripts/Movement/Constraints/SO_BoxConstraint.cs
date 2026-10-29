using Unity.Mathematics.Geometry;
using UnityEngine;

[CreateAssetMenu(fileName = "Box Constraint", menuName = "ScriptableObjects/Constraints/Box Constraint")]
public class SO_BoxConstraint : SO_GridConstraint
{
    [SerializeField]
    private MinMaxAABB AABB;
    [SerializeField]
    private bool MustBeInside = true;

    public override bool IsPositionValid(Transform source, Vector3 position, SO_GridSettings gridSettings)
    {
        return MustBeInside ? AABB.Contains(position) : !AABB.Contains(position);
    }
}
