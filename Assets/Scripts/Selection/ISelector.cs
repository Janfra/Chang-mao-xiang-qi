using UnityEngine.Events;

public interface ISelector
{
    event UnityAction OnSelectionChanged;
    SelectableComponent CurrentSelected { get; }
}
