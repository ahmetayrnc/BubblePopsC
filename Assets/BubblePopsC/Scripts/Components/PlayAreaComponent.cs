using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace BubblePopsC.Scripts.Components
{
    [Game, Unique]
    public class PlayAreaComponent : IComponent
    {
        public Vector2 Center;
        public Vector2 Size;

        public Vector2 RightWallStart;
        public Vector2 RightWallEnd;
        public Vector2 LeftWallStart;
        public Vector2 LeftWallEnd;
    }
}