using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Input
{
    [Input]
    public sealed class TouchDownComponent : IComponent
    {
        public Vector2 Value;
    }
}