using UnityEngine;

public abstract class SO_MovementType : ScriptableObject
{
    public SO_GridSettings GridSettings;
    public abstract Vector2[] GetMovePositionsAtSteps(Vector2 origin, int steps);
}
