using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Position;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Board
{
    [Game, Unique]
    public class CeilingCoordsComponent : IComponent
    {
        public List<AxialCoord> Value;
    }
}