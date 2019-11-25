using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Bubble
{
    [Game, Event(EventTarget.Self)]
    public class GhostComponent : IComponent
    {
    }
}