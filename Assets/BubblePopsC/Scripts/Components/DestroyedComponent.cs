using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components
{
    [Game, Event(EventTarget.Self)]
    public class DestroyedComponent : IComponent
    {
    }
}