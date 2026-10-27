using UnityEngine;

public static class GridExtensions
{
    public const int GridLength = 10;
    public const int GridWidth = 9;

    public static Vector3 SnapToGrid(this Vector3 position, float gridSize)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }

    public static bool IsAlignedToGrid(this Vector3 position, float gridSize)
    {
        return Mathf.Approximately(position.x % gridSize, 0f) &&
               Mathf.Approximately(position.y % gridSize, 0f) &&
               Mathf.Approximately(position.z % gridSize, 0f);
    }

    public static Vector2 SnapToGrid(this Vector2 position, float gridSize)
    {
        return new Vector2(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize
        );
    }

    public static bool IsAlignedToGrid(this Vector2 position, float gridSize)
    {
        return Mathf.Approximately(position.x % gridSize, 0f) &&
               Mathf.Approximately(position.y % gridSize, 0f);
    }
}
