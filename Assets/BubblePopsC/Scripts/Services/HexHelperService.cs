using UnityEngine;

namespace BubblePopsC.Scripts.Services
{
    public static class HexHelperService
    {
        public static Vector2 HexToPoint(int q, int r)
        {
            const float size = 0.5f;
            var x = size * (Mathf.Sqrt(3f) * q + Mathf.Sqrt(3f) / 2f * r);
            var y = size * (3f / 2 * r);
            return new Vector2(x, y);
        }
    }
}