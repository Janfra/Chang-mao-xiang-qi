using UnityEngine;

[CreateAssetMenu(fileName = "Collision Constraint", menuName = "ScriptableObjects/Constraints/Collision Constraint")]
public class SO_CollisionConstraint : SO_GridConstraint
{
    [SerializeField]
    private LayerMask _collisionLayerMask;
    [SerializeField]
    private float _collisionCheckRadius = 1;

    public override bool IsPositionValid(Transform source, Vector3 position, SO_GridSettings gridSettings)
    {
        return !Physics2D.OverlapCircle(position, _collisionCheckRadius, _collisionLayerMask); // Check for collisions at the target position
    }
}
