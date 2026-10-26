using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelectorComponent : MonoBehaviour
{
    [SerializeField]
    private LayerMask _selectableLayerMask;
    [SerializeField]
    private bool _allowDeselectOnRepeatSelect = true;

    private SelectableComponent _currentSelected;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySelectAtMousePosition();
        }
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
            if (_currentSelected == selectable)
            {
                if (_allowDeselectOnRepeatSelect)
                {
                    ClearLastSelection();
                }
                return;
            }

            ClearLastSelection();
            _currentSelected = selectable;
            selectable.OnSelected();
        }
    }

    private void ClearLastSelection()
    {
        if (_currentSelected != null)
        {
            _currentSelected.OnDeselected();
            _currentSelected = null;
        }
    }
}
