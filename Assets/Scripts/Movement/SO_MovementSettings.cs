using UnityEngine;

[CreateAssetMenu(fileName = "Movement Settings", menuName = "ScriptableObjects/Movement Settings")]
public class SO_MovementSettings : ScriptableObject
{
    [field: SerializeField]
    public SO_GridSettings GridSettings { get; private set; }

    [field: SerializeField]
    public float MoveSpeed { get; private set; } = 5f;

    [field: SerializeField]
    [field: Range(1, 10)]
    public int MaxMoveSteps { get; private set; } = 1;
}
