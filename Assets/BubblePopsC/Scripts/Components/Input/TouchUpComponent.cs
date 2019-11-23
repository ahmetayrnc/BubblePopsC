using System.Numerics;
using Entitas;

namespace BubblePopsC.Scripts.Components.Input
{
    [Input]
    public sealed class TouchUpComponent : IComponent
    {
        public Vector2 Value;
    }
}