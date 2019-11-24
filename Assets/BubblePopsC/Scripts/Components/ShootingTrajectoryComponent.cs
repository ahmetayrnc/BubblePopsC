using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using EventType = Entitas.CodeGeneration.Attributes.EventType;

namespace BubblePopsC.Scripts.Components
{
    [Game, Unique, Event(EventTarget.Any), Event(EventTarget.Any, EventType.Removed)]
    public class ShootingTrajectoryComponent : IComponent
    {
        public List<Vector3> Points;
    }
}