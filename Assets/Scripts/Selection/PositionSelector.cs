using UnityEngine;
using System;
using UnityEngine.Pool;
using UnityEngine.Events;
using System.Collections.Generic;

[Serializable]
public class PositionSelector
{
    [HideInInspector]
    public Transform InstancesParent;

    [SerializeField]
    private PositionOptionComponent _positionOptionPrefab;

    private ObjectPool<PositionOptionComponent> _instances;
    private UnityAction<Vector2> _onPositionSelect;
    private List<PositionOptionComponent> _shown;

    public void Awake(UnityAction<Vector2> onPositionSelect)
    {
        _onPositionSelect = onPositionSelect;
        _instances = new(Spawn, OnGet, OnRelease, OnDestroy);
        _shown = new List<PositionOptionComponent>();
    }

    private void OnDestroy(PositionOptionComponent component)
    {
        
    }

    private void OnRelease(PositionOptionComponent component)
    {
        component.gameObject.SetActive(false);
        component.OnReleased = null;
        component.OnPositionSelected -= _onPositionSelect;
    }

    private void OnGet(PositionOptionComponent component)
    {
        component.gameObject.SetActive(true);
        component.OnReleased = _instances.Release;
        component.OnPositionSelected += _onPositionSelect;
    }

    private PositionOptionComponent Spawn()
    {
        var instance = MonoBehaviour.Instantiate(_positionOptionPrefab, InstancesParent);
        instance.gameObject.SetActive(false);
        return instance;
    }

    public void ShowOptionsAt(Vector2[] positions)
    {
        foreach (var position in positions)
        {
            var component = _instances.Get();
            component.transform.position = position;
            _shown.Add(component);
        }
    }

    public void HideAll()
    {
        for (int i = _shown.Count - 1; i >= 0; i--)
        {
            var component = _shown[i];
            component.Release();
            _shown.RemoveAt(i);
        }
    }
}
