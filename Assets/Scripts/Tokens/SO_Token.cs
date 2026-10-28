using UnityEngine;

[CreateAssetMenu(fileName = "Token", menuName = "ScriptableObjects/Token")]
public class SO_Token : ScriptableObject
{
    [field: SerializeField]
    public Sprite Icon;

    [field: SerializeField]
    public Color Background;
}
