using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/Health")]
public class SO_Health : ScriptableObject
{
    [field: SerializeField]
    public int MaxHealth { get; private set; } = 1;
}
