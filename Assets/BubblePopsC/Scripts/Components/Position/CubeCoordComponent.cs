using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Position
{
    [Game, Event(EventTarget.Self)]
    public class AxialCoordComponent : IComponent
    {
        public int Q;
        public int R;
    }
}