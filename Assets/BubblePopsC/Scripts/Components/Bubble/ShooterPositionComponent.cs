using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Bubble
{
    [Game, Unique]
    public class ShooterPositionComponent : IComponent
    {
        public Vector2 Value;
    }

    [Game, Unique]
    public class SparePositionComponent : IComponent
    {
        public Vector2 Value;
    }
}