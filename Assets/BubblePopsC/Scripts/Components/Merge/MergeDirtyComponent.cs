using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Merge
{
    [Game, Event(EventTarget.Self)]
    public class MergeDirtyComponent : IComponent
    {
    }

    [Game, Unique]
    public class MergingComponent : IComponent
    {
    }
}