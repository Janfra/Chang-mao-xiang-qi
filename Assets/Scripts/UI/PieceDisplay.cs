using UnityEngine;
using UnityEngine.UI;

public class PieceDisplay : MonoBehaviour
{
    [field: SerializeField]
    public Image UnitIcon { get; private set; }

    [field: SerializeField]
    public Image ActionIcon { get; private set; }

    [SerializeField]
    private SO_SelectedNotifier _selectNotifier;

    [HideInInspector]
    public SelectableComponent AssociatedSelection;

    public void TrySelect()
    {
        if (AssociatedSelection != null)
        {
            _selectNotifier.Selector.RequestSelection(AssociatedSelection);
        }
    }
}
