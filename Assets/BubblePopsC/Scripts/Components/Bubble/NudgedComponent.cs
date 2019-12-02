using System;
using BubblePopsC.Scripts.Components.Position;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Bubble
{
    [Game, Event(EventTarget.Self)]
    public class NudgedComponent : IComponent
    {
        public AxialCoord From;
        public Action Callback;
    }
}