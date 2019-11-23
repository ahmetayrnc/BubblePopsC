using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Input
{
    [Input]
    public sealed class TouchPositionComponent : IComponent
    {
        public Vector2 Value;
    }
}