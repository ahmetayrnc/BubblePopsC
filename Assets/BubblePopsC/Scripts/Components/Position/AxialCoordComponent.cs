using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Position
{
    [Game, Event(EventTarget.Self)]
    public class AxialCoordComponent : IComponent
    {
        public AxialCoord Value;

        public override string ToString()
        {
            return $"(q:{Value.Q}, r:{Value.R})";
        }
    }

    public struct AxialCoord
    {
        public int Q;
        public int R;
    }
}