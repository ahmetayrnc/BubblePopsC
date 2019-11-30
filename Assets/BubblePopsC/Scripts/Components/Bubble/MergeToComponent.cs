using System;
using BubblePopsC.Scripts.Components.Position;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Bubble
{
    [Game, Event(EventTarget.Self)]
    public class MergeToComponent : IComponent
    {
        public AxialCoord Spot;
        public Action Callback;
        public bool IsMaster;
    }
}