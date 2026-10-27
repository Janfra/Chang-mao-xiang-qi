using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SelectableComponent))]
public class PositionOptionComponent : MonoBehaviour
{
    public event UnityAction<Vector2> OnPositionSelected;

    public bool IsAutoReleased = true;
    public UnityAction<PositionOptionComponent> OnReleased;

    private SelectableComponent _selectable;

    private void Awake()
    {
        _selectable = GetComponent<SelectableComponent>();
        _selectable.OnSelected += OnSelect;
    }

    private void OnDisable()
    {
        OnPositionSelected = null;
    }

    public void Release()
    {
        OnReleased?.Invoke(this);
    }

    private void OnSelect()
    {
        OnPositionSelected?.Invoke(transform.position);
        if (IsAutoReleased)
        {
            Release();
        }
    }
}
