using System;
using System.Collections.Generic;
using UnityEngine;

public class PiecesAvailableDisplay : MonoBehaviour
{
    [SerializeField]
    private SO_PlayerUnitRegistry _playerUnits;
    [SerializeField]
    private PieceDisplay _displayPrefab;
    private Dictionary<PlayerUnit, PieceDisplay> _displayDict = new();
    private List<PieceDisplay> _displayList = new();

    private bool _canDisplay = true;

    private void Start()
    {
        foreach (var unit in _playerUnits.PlayerUnits)
        {
            var display = Instantiate(_displayPrefab, transform);
            display.UnitIcon.sprite = unit.UnitUIIcon;
            display.AssociatedSelection = unit.SelectableComponent;
            display.ActionIcon.sprite = unit.AvailablePlayerAction.Icon;

            _displayDict.Add(unit, display);
            _displayList.Add(display);
            unit.OnAvailableActionUpdated += UpdateUnitDisplay;
        }   
    }

    public void HideAll()
    {
        _canDisplay = false;
        foreach (var item in _displayList)
        {
            item.gameObject.SetActive(false);
            item.ActionIcon.sprite = null;
        }
    }

    public void ShowValid()
    {
        _canDisplay = true;
        foreach (var item in _displayList)
        {
            if (item.ActionIcon.sprite != null)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateUnitDisplay(PlayerUnit unit)
    {
        if (_displayDict.TryGetValue(unit, out var display))
        {
            if (unit.AvailablePlayerAction == null)
            {
                display.gameObject.SetActive(false);
                display.ActionIcon.sprite = null;
                return;
            }

            var newIcon = unit.AvailablePlayerAction.Icon;
            display.ActionIcon.sprite = newIcon;
            if (newIcon == null)
            {
                display.gameObject.SetActive(false);
            }
            else if (_canDisplay)
            {
                display.gameObject.SetActive(true);
            }
        }
    }
}
