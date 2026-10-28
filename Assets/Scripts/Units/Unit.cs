using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected SO_MovementType _movementType;
    [SerializeField] protected GridRestrictionComponent _gridRestrictionComponent;
    
    [field: SerializeField] 
    public Sprite UnitUIIcon { get; private set; }

    [field: SerializeField]
    public HealthComponent HealthComponent { get; private set; }
    
    [field: SerializeField]
    public SO_MovementSettings MoveSettings { get; private set; }

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
    protected Vector2[] GetPossiblePositions()
    {
        var positions = _movementType.GetMovePositionsAtSteps(transform.position, MoveSettings ? MoveSettings.MaxMoveSteps : 1);
        if (_gridRestrictionComponent)
        {
            positions = _gridRestrictionComponent.FilterValidPositions(positions);
        }
        return positions;
    }

    protected abstract void OnDie();
}
