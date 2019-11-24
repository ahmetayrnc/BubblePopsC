using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Input
{
    [Input, Unique]
    public sealed class TouchPositionComponent : IComponent
    {
        public Vector2 Value;
    }
}