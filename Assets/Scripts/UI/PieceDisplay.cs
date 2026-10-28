using UnityEngine;
using UnityEngine.UI;

public class PieceDisplay : MonoBehaviour
{
    [field: SerializeField]
    public Image UnitIcon { get; private set; }

    [field: SerializeField]
    public Image ActionIcon { get; private set; }

    [HideInInspector]
    public SelectableComponent AssociatedSelection;
}
