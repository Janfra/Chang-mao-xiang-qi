using UnityEngine.Events;

public interface ISelector
{
    event UnityAction OnSelectionChanged;
    SelectableComponent CurrentSelected { get; }
    public void RequestSelection(SelectableComponent selectedComponent);
}
