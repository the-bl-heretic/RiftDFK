using UnityEngine;

namespace RiftDFK
{
    public static class RiftMath
    {
        /// <summary>
        /// Fast Inverse Square Root (basic version)
        /// </summary>
        public static float InverseSqrt(float x)
        {
            return 1.0f / Mathf.Sqrt(x);
        }

        /// <summary>
        /// Clamp a value between min and max
        /// </summary>
        public static float Clamp(float value, float min, float max)
        {
            return Mathf.Max(min, Mathf.Min(max, value));
        }

        // 🚀 SUVAT Physics Equations

        /// <summary>
        /// Final Velocity (v) from Initial Velocity (u), Acceleration (a), Time (t)
        /// v = u + at
        /// </summary>
        public static float SolveV(float u, float a, float t)
        {
            return u + (a * t);
        }

        /// <summary>
        /// Displacement (s) from Initial Velocity (u), Time (t), Acceleration (a)
        /// s = ut + (1/2)at²
        /// </summary>
        public static float SolveS(float u, float a, float t)
        {
            return (u * t) + (0.5f * a * t * t);
        }

        /// <summary>
        /// Final Velocity squared (v²) from Initial Velocity squared (u²), Acceleration (a), Displacement (s)
        /// v² = u² + 2as
        /// </summary>
        public static float SolveV2(float u, float a, float s)
        {
            return (u * u) + (2f * a * s);
        }

        /// <summary>
        /// Displacement (s) from (u + v)/2 * t
        /// s = ((u + v) / 2) * t
        /// </summary>
        public static float SolveS2(float u, float v, float t)
        {
            return ((u + v) / 2f) * t;
        }

        /// <summary>
        /// Displacement (s) using v and a
        /// s = vt - (1/2)at²
        /// </summary>
        public static float SolveS3(float v, float a, float t)
        {
            return (v * t) - (0.5f * a * t * t);
        }
    }
}
