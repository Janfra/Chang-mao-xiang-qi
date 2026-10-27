using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class SelectableComponent : MonoBehaviour
{
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

    public void OnSelected()
    {
        if (CanBeFocused)
        {
            IsSelected = true;
        }
        _onSelected?.Invoke();
    }
    
    public void OnDeselected()
    {
        IsSelected = false;
        _onDeselected?.Invoke();
    }
}
