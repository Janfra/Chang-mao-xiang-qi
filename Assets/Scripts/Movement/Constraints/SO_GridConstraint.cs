using UnityEngine;

public abstract class SO_GridConstraint : ScriptableObject
{
    public abstract bool IsPositionValid(Transform source, Vector3 position, SO_GridSettings gridSettings);
}
