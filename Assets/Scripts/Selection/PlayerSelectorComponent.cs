using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSelectorComponent : MonoBehaviour, ISelector
{
    public event UnityAction OnSelectionChanged;

    [SerializeField]
    private LayerMask _selectableLayerMask;
    [SerializeField]
    private bool _allowDeselectOnRepeatSelect = true;
    [SerializeField]
    private SO_SelectedNotifier _selectedNotifier;

    public SelectableComponent CurrentSelected { get; private set;}

    private void Awake()
    {
        if (_selectedNotifier != null)
        {
            _selectedNotifier.AssignSelector(this);
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySelectAtMousePosition();
        }
    }

    public void RequestSelection(SelectableComponent selectable)
    {
        TrySelect(selectable);
    }

    public void TrySelectAtMousePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        TrySelectAtPosition(mousePosition);
    }

    public void TrySelectAtPosition(Vector2 position)
    {
        var collider = Physics2D.OverlapPoint(position, _selectableLayerMask.value);
        if (collider != null)
        {
            var selectable = collider.GetComponent<SelectableComponent>();
            TrySelect(selectable);
        }
    }

    public void TrySelect(SelectableComponent selectable)
    {
        if (selectable != null)
        {
            if (selectable.CanBeFocused)
            {
                TrySetAsFocused(selectable);
            }
            else
            {
                TriggerSelectable(selectable);
            }
        }
    }

    public void Deselect()
    {
        ClearLastSelection();
        OnSelectionChanged?.Invoke();
    }

    private void TriggerSelectable(SelectableComponent selectable)
    {
        selectable.OnSelect();
    }

    private void TrySetAsFocused(SelectableComponent selectable)
    {
        if (CurrentSelected == selectable)
        {
            if (_allowDeselectOnRepeatSelect)
            {
                Deselect();
            }
            return;
        }

        // New selectable, doesnt use Deselect to avoid double invoking OnSelectionChanged
        ClearLastSelection();
        CurrentSelected = selectable;
        selectable.OnSelect();
        OnSelectionChanged?.Invoke();
    }

    private void ClearLastSelection()
    {
        if (CurrentSelected != null)
        {
            CurrentSelected.OnDeselect();
            CurrentSelected = null;
        }
    }
}
