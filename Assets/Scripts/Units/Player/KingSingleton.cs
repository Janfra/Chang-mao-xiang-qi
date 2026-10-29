using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerUnit))]
public class KingSingleton : MonoBehaviour
{
    public event UnityAction OnKingDied;

    [SerializeField] private SO_TurnNotifier SO_turn;
    public static KingSingleton Instance { get; private set; }
    private PlayerUnit _kingUnit;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _kingUnit = GetComponent<PlayerUnit>();
    }

    private void Start()
    {
        _kingUnit.HealthComponent.Health.OnDied += OnKingDeath;
    }

    private void OnDestroy()
    {
        _kingUnit.HealthComponent.Health.OnDied -= OnKingDeath;
    }

    public Vector2 GetKingPos()
    {
        return transform.position;
    }

    public void Endturn(InputAction.CallbackContext call)
    {

        if (call.started)
        {

            SO_turn.TurnManager.RequestEndPlayerTurn();
        }
        
    }

    private void OnKingDeath()
    {
        OnKingDied?.Invoke();
    }
}
