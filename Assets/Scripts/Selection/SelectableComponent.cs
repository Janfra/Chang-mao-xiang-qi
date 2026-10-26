using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class SelectableComponent : MonoBehaviour
{
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
        _onSelected?.Invoke();
        IsSelected = true;
    }
    
    public void OnDeselected()
    {
        _onDeselected?.Invoke();
        IsSelected = false;
    }
}
