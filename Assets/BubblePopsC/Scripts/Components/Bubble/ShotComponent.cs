using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace BubblePopsC.Scripts.Components.Bubble
{
    [Game, Event(EventTarget.Self)]
    public class ShotComponent : IComponent
    {
        public Vector3[] Trajectory;
        public Action Callback;
    }
}