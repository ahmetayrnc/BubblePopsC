using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace BubblePopsC.Scripts.Components.Position
{
    [Game, Event(EventTarget.Self)]
    public class AxialCoordComponent : IComponent
    {
        public AxialCoord Value;

        public override string ToString()
        {
            return $"(q:{Value.Q}, r:{Value.R})";
        }
    }

    public class AxialCoord
    {
        public int Q;
        public int R;

        public override string ToString()
        {
            return $"(q:{Q}, r:{R})";
        }

        public bool Equals(AxialCoord p)
        {
            // If parameter is null, return false.
            if (ReferenceEquals(p, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, p))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (GetType() != p.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return Q == p.Q && R == p.R;
        }
    }
}