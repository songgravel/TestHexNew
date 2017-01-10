using UnityEngine;

namespace CobraGame
{
    public enum HexDirection
    {
        NE, E, SE, SW, W, NW
    }

    public static class HexDirectionExtensions
    {
        public static HexDirection Opposite(this HexDirection direction)
        {
            return (int)direction < 3 ? (direction + 3) : (direction - 3);
        }

        public static HexDirection Previous(this HexDirection direction)
        {
            return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
        }

        public static HexDirection Next(this HexDirection direction)
        {
            return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
        }
    }

    public enum HexMapData
    {
        ENTRY = 1,
        BASE = 2,
        JOINT = 4,
        BUILD = 8,
        PATH = 16
    }

    public static class HexMetrics
    {
        private const float SQRT32 = 0.86602540378443864676372317075294f;   //sqrt(3)/2

        public static float outerRadius = 10f;

        public static float innerRadius = outerRadius * SQRT32;

        public const float solidFactor = 0.9f;

        public const float blendFactor = 1f - solidFactor;

        public const float elevationStep = 1f;

        public static int chunkSizeX = 5, chunkSizeZ = 5;

        static Vector3[] corners = {
            new Vector3(0f, 0f, outerRadius),
            new Vector3(innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(0f, 0f, -outerRadius),
            new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f, 0f, outerRadius)
        };

        public static void UpdateRadius()
        {
            innerRadius = outerRadius * SQRT32;
            corners[0] = new Vector3(0f, 0f, outerRadius);
            corners[1] = new Vector3(innerRadius, 0f, 0.5f * outerRadius);
            corners[2] = new Vector3(innerRadius, 0f, -0.5f * outerRadius);
            corners[3] = new Vector3(0f, 0f, -outerRadius);
            corners[4] = new Vector3(-innerRadius, 0f, -0.5f * outerRadius);
            corners[5] = new Vector3(-innerRadius, 0f, 0.5f * outerRadius);
            corners[6] = new Vector3(0f, 0f, outerRadius);
        }

        public static Vector3 GetFirstCorner(HexDirection direction)
        {
            return corners[(int)direction];
        }

        public static Vector3 GetSecondCorner(HexDirection direction)
        {
            return corners[(int)direction + 1];
        }

        public static Vector3 GetFirstSolidCorner(HexDirection direction)
        {
            return corners[(int)direction] * solidFactor;
        }

        public static Vector3 GetSecondSolidCorner(HexDirection direction)
        {
            return corners[(int)direction + 1] * solidFactor;
        }

        public static Vector3 GetBridge(HexDirection direction)
        {
            return (corners[(int)direction] + corners[(int)direction + 1]) *
                blendFactor;
        }
    }
}
