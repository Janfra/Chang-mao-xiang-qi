using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class SelectableComponent : MonoBehaviour
{
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
    }
    
    public void OnDeselected()
    {
        _onDeselected?.Invoke();
    }
}
