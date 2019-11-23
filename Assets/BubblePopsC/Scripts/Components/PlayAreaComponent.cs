using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components
{
    [Game, Unique]
    public class PlayAreaComponent : IComponent
    {
        public float Width;
        public float Height;
    }
}