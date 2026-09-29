using L8_3DGED_CSharp_Scratch;
using System;

namespace Engine
{
    /// <summary>
    /// Provides general-purpose maths utility methods used across the engine (e.g. range validation and clamping).
    /// </summary>
    public static class GDMath
    {
        #region Static Methods

        public static void Swap(ref int x, ref int y)
        {
            Console.WriteLine($"Inside before - x: {x}, y: {y}");

            int temp = x;
            x = y;
            y = temp;

            Console.WriteLine($"Inside after - x: {x}, y: {y}");
        }

        public static float Lerp(float min, float max, float t)
        {
            return min + (max - min) * t;
        }

        public static double Lerp(double min, double max, double t)
        {
            return min + (max - min) * t;
        }

        public static Vector3 Lerp(Vector3 min, Vector3 max, float t)
        {
            return new Vector3(Lerp(min.X, max.X, t), 
                Lerp(min.Y, max.Y, t), 
                Lerp(min.Z, max.Z, t));
        }           





        /// <summary>
        /// Validates that a value lies within an inclusive range and, if it does not, substitutes a caller-supplied default.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="min">The inclusive lower bound of the valid range.</param>
        /// <param name="max">The inclusive upper bound of the valid range.</param>
        /// <param name="defaultValue">The value returned when <paramref name="value"/> falls outside the range.</param>
        /// <returns><paramref name="value"/> when in range, otherwise <paramref name="defaultValue"/>.</returns>
        /// <remarks>
        /// Note for students: despite the name, this is NOT a mathematical clamp. A value of -0.5 with a
        /// range of [0, 1] and a default of 1 returns 1, not 0. Use this overload when an out-of-range
        /// value indicates programmer error and a known-good fallback is wanted; use
        /// <see cref="Clamp(float, float, float)"/> when the value should be pulled to the nearest bound.
        /// </remarks>
        public static float ClampToDefault(float value, float min, float max, float defaultValue)
        {
            return value >= min && value <= max ? value : defaultValue;
        }

        /// <summary>
        /// Clamps a value to an inclusive range by returning the nearest bound when the value falls outside it.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The inclusive lower bound of the valid range.</param>
        /// <param name="max">The inclusive upper bound of the valid range.</param>
        /// <returns><paramref name="min"/> if the value is below the range, <paramref name="max"/> if above, otherwise the value itself.</returns>
        /// <remarks>
        /// This is the conventional clamp (cf. UnityEngine.Mathf.Clamp) and is the correct choice when
        /// arithmetic legitimately overshoots a range, e.g. adding two colours and saturating at 1.
        /// </remarks>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        #endregion
    }
}
