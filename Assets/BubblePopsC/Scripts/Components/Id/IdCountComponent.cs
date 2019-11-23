using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Id
{
    [Game, Unique]
    public class IdCountComponent : IComponent
    {
        public int Value;
    }
}