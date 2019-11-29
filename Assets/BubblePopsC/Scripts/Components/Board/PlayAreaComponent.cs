using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Board
{
    [Game, Unique]
    public class PlayAreaComponent : IComponent
    {
        public Vector2 Center;
        public Vector2 Size;
    }
}