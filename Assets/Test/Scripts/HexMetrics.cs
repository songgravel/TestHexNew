using UnityEngine;

namespace CobraGame
{
    public static class HexMetrics
    {
        private const float SQRT32 = 0.86602540378443864676372317075294f;   //sqrt(3)/2

        public const float outerRadius = 10f;

        public const float innerRadius = outerRadius * SQRT32;

        public static Vector3[] corners = {
            new Vector3(0f, 0f, outerRadius),
            new Vector3(innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(0f, 0f, -outerRadius),
            new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius)
        };
    }
}
