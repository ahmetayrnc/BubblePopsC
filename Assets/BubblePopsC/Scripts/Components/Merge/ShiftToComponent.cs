using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Merge
{
    [Game, Event(EventTarget.Self)]
    public class ShiftToComponent : IComponent
    {
        public Vector2 Spot;
        public Action Callback;
    }
}