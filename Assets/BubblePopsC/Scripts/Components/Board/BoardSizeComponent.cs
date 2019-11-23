using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Board
{
    [Game, Unique, Event(EventTarget.Any)]
    public class BoardSizeComponent : IComponent
    {
        public Vector2Int Value;
    }
}