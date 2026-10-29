using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected SO_MovementType _movementType;
    [field: SerializeField]
    public SO_MovementSettings MoveSettings { get; private set; }
    [SerializeField] protected GridRestrictionComponent _gridRestrictionComponent;
    [field: SerializeField] 
    public Sprite UnitUIIcon { get; private set; }

    [field: Header("Required Components")]
    [field: SerializeField]
    public HealthComponent HealthComponent { get; private set; }

    public bool IsDead => HealthComponent.Health.IsDead;

    protected virtual void Start()
    {
        HealthComponent.Health.OnDied += OnDie;
    }

    private void OnDestroy()
    {
        HealthComponent.Health.OnDied -= OnDie;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected Vector2[] GetPossiblePositions(int steps)
    {
        var positions = _movementType.GetMovePositionsAtSteps(transform.position, steps);
        if (_gridRestrictionComponent)
        {
            positions = _gridRestrictionComponent.FilterValidPositions(positions);
        }
        return positions;
    }

    protected Vector2[] GetPossiblePositions()
    {
        return GetPossiblePositions(MoveSettings ? MoveSettings.MaxMoveSteps : 1);
    }

    protected abstract void OnDie();
}
