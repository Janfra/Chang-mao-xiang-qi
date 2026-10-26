using UnityEngine;

// Ordered to easily cycle through straight movement and diagonal movement
public enum Direction
{
    Up,
    Right,
    Down,
    Left,
    UpLeft,
    UpRight,
    DownRight,
    DownLeft,
}

public static class DirectionExtensions
{
    public static Vector3 ToVector3(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Vector3(0, 1, 0),
            Direction.UpRight => new Vector3(1, 1, 0).normalized,
            Direction.UpLeft => new Vector3(-1, 1, 0).normalized,
            Direction.Down => new Vector3(0, -1, 0),
            Direction.DownRight => new Vector3(1, -1, 0).normalized,
            Direction.DownLeft => new Vector3(-1, -1, 0).normalized,
            Direction.Left => new Vector3(-1, 0, 0),
            Direction.Right => new Vector3(1, 0, 0),
            _ => throw new System.ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
