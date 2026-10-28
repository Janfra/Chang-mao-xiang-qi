using UnityEngine;

#if UNITY_EDITOR
#endif

[CreateAssetMenu(fileName = "Player Action", menuName = "ScriptableObjects/Player Action")]
public class SO_PlayerActionDescription : ScriptableObject
{
    public Sprite Icon;
}
