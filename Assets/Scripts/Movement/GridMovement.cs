using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Movement component that aligns with grid, allowing movement in 8 directions (up, down, left, right, and diagonals).
/// <remarks>
/// Does not use physics for movement, so it is not affected by Unity's physics engine. Does not perform any collision detection or response, any overlapping with other objects will need to be handled separately before movement is started since we are going for turn-based.
/// </remarks>
/// </summary>
public class GridMovement : MonoBehaviour
{
    public event UnityAction OnMoveCompleted;

    [field: SerializeField]
    public SO_MovementSettings MovementSettings { get; private set; }

    public bool IsMoving { get; private set; } = false;
    private Vector3 _targetPosition;
    public Vector2 Position => new Vector2(transform.position.x, transform.position.y);

    private void Awake()
    {
        if (MovementSettings == null)
        {
            Debug.LogError("MovementSettings is not assigned in GridMovement.");
            if (MovementSettings.GridSettings == null)
            {
                Debug.LogError("GridSettings is not assigned in MovementSettings.");
            }
        }

        CheckAlignment();
    }

    [ContextMenu("Check Alignment")]
    private void CheckAlignment()
    {
        if (!transform.position.IsAlignedToGrid(MovementSettings.GridSettings.GridSize))
        {
            Debug.LogWarning("Object is not aligned to the grid. Consider snapping it to the nearest grid point.");
        }
    }

    private void FixedUpdate()
    {
        // Not physics-based so not needed, but may be useful if we add physics later
        if (IsMoving)
        {
            MoveTowardsTarget();
        }
    }

    public void MoveTo(Vector2 position)
    {
        if (IsMoving)
        {
            Debug.LogWarning("Already moving. Cannot initiate another move.");
            return;
        }

        if (!position.IsAlignedToGrid(MovementSettings.GridSettings.GridSize))
        {
            Debug.LogWarning("Target position is not aligned to the grid. Will be ignored, consider snapping it to grid first.");
            return;
        }

        _targetPosition = position;
        IsMoving = true;
    }

    private void MoveTowardsTarget()
    {
        // Not using physics for now for simplicity
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, MovementSettings.MoveSpeed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
        {
            IsMoving = false;
            transform.position = _targetPosition;
            OnMoveCompleted?.Invoke();
        }
    }
}
