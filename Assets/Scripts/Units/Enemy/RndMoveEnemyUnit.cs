using UnityEngine;

public class RndMoveEnemyUnit : EnemyUnit
{
        protected override Vector2 GetBestMove()
        {
            var positions = GetPossiblePositions();
            return positions[Random.Range(0, positions.Length)];
        }

}
