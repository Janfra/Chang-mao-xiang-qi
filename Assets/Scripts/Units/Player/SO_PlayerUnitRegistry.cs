using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Unit Registry", menuName = "ScriptableObjects/Registry/Player Units")]
public class SO_PlayerUnitRegistry : ScriptableObject
{
    public List<PlayerUnit> PlayerUnits { get; private set; } = new();

    private void OnEnable()
    {
        PlayerUnits.Clear();
    }

    private void OnDisable()
    {
        PlayerUnits.Clear();
    }

    public void Register(PlayerUnit unit)
    {
        if (PlayerUnits.Contains(unit))
        {
            return;
        }

        PlayerUnits.Add(unit);
    }

    public void Remove(PlayerUnit unit)
    {
        PlayerUnits.Remove(unit);
    }
}
