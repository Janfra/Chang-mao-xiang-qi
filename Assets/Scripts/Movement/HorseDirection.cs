using UnityEngine;

public enum HorseDirection
{
    UpLeft,
    UpRight,
    RightUp,
    RightDown,
    DownRight,
    DownLeft,
    LeftDown,
    LeftUp,
    DIRECTION_COUNT // Use only for counting purposes
}

public static class HorseDirectionsExtensions
{
    public static Vector2 ToVector2(this HorseDirection direction)
    {
        return direction switch
        {
            HorseDirection.UpLeft => Direction.Up.ToVector3() + Direction.UpLeft.ToVector3(),
            HorseDirection.UpRight => Direction.Up.ToVector3() + Direction.UpRight.ToVector3(),
            HorseDirection.RightUp => Direction.Right.ToVector3() + Direction.UpRight.ToVector3(),
            HorseDirection.RightDown => Direction.Right.ToVector3() + Direction.DownRight.ToVector3(),
            HorseDirection.DownRight => Direction.Down.ToVector3() + Direction.DownRight.ToVector3(),
            HorseDirection.DownLeft => Direction.Down.ToVector3() + Direction.DownLeft.ToVector3(),
            HorseDirection.LeftDown => Direction.Left.ToVector3() + Direction.DownLeft.ToVector3(),
            HorseDirection.LeftUp => Direction.Left.ToVector3() + Direction.UpLeft.ToVector3(),
            _ => throw new System.ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}