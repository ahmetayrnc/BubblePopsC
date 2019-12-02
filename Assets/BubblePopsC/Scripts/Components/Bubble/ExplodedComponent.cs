using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Bubble
{
    [Game, Event(EventTarget.Self)]
    public class ExplodedComponent : IComponent
    {
        public Action Callback;
        public bool IsMaster;
    }
}