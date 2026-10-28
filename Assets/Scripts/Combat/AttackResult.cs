using System;
using UnityEngine;

public class AttackResult
{
    public Vector2 Position;
    public HealthComponent HitHealth;

    public AttackResult(Vector2 position,  HealthComponent hitHealth)
    {
        Position = position;
        HitHealth = hitHealth;
    }

    public override bool Equals(object obj)
    {
        return obj is AttackResult result && result.Position == Position && result.HitHealth == HitHealth;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, HitHealth);
    }
}
