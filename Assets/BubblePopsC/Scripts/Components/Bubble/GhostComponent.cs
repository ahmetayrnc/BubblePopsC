using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Bubble
{
    [Game, Event(EventTarget.Self), Event(EventTarget.Self, EventType.Removed)]
    public class GhostComponent : IComponent
    {
    }
}