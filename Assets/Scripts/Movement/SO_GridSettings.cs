using UnityEngine;

[CreateAssetMenu(fileName = "Grid Settings", menuName = "ScriptableObjects/Grid Settings")]
public class SO_GridSettings : ScriptableObject
{
    // Should match the grid size settings in the editor
    [field: SerializeField] 
    public float GridSize { get; private set; } = 1f;
}
