using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Board
{
    [Game, Unique, Event(EventTarget.Any)]
    public class BoardOffset : IComponent
    {
        public bool Indented;
    }
}