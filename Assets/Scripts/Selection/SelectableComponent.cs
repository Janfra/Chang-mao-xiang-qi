using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class SelectableComponent : MonoBehaviour
{
    public event UnityAction OnSelected
    {
        add => _onSelected.AddListener(value);
        remove => _onSelected.RemoveListener(value);
    }

    public event UnityAction OnDeselected
    {
        add => _onDeselected.AddListener(value);
        remove => _onDeselected.RemoveListener(value);
    }

    [field: SerializeField]
    [field: Tooltip("Will only trigger selected when active. Only focusable objects will be set as the current selected when interacted with.")]
    public bool CanBeFocused { get; private set; }

    public bool IsSelected { get; private set; }    

    [SerializeField]
    private UnityEvent _onSelected;
    
    [SerializeField]
    private UnityEvent _onDeselected;

    private void Awake()
    {
        var collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
    }

    public void OnSelect()
    {
        if (CanBeFocused)
        {
            IsSelected = true;
        }
        _onSelected?.Invoke();
    }
    
    public void OnDeselect()
    {
        IsSelected = false;
        _onDeselected?.Invoke();
    }
}
