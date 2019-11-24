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
}