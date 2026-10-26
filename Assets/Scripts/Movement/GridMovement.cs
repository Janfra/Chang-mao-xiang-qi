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

    public SO_GridSettings GridSettings;
    public float MoveSpeed = 5f;

    private Vector3 _targetPosition;
    private bool _isMoving = false;

    private void Awake()
    {
        if (GridSettings == null)
        {
            Debug.LogError("GridSettings is not assigned in GridMovement.");
        }

        CheckAlignment();
    }

    [ContextMenu("Check Alignment")]
    private void CheckAlignment()
    {
        if (!transform.position.IsAlignedToGrid(GridSettings.GridSize))
        {
            Debug.LogWarning("Object is not aligned to the grid. Consider snapping it to the nearest grid point.");
        }
    }

    private void FixedUpdate()
    {
        // Not physics-based so not needed, but may be useful if we add physics later
        if (_isMoving)
        {
            MoveTowardsTarget();
        }
    }

    public void MoveTo(Direction direction, int distance = 1)
    {
        if (_isMoving)
        {
            Debug.LogWarning("Already moving. Cannot initiate another move.");
            return;
        }

        distance = Mathf.Max(1, distance); // Ensure distance is at least 1
        _targetPosition = transform.position + direction.ToVector3() * (distance * GridSettings.GridSize);
        _isMoving = true;
    }

    private void MoveTowardsTarget()
    {
        // Not using physics for now for simplicity
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, MoveSpeed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
        {
            _isMoving = false;
            transform.position = _targetPosition;
            OnMoveCompleted?.Invoke();
        }
    }
}
