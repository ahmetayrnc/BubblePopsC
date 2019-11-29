using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Id
{
    [Game]
    public class IdComponent : IComponent
    {
        [PrimaryEntityIndex] public int Value;
    }
}