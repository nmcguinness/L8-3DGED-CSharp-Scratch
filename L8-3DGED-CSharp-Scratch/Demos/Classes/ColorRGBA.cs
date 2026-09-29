using Engine;
using L8_3DGED_CSharp_Scratch;

namespace Graphics
{
    /// <summary>
    /// Represents a colour with four floating-point channels (red, green, blue and alpha), each in the range 0 to 1.
    /// </summary>
    /// <remarks>
    /// Original specification for this class:
    ///  1. Store colours for 4 channels (Red, Green, Blue, Alpha) as floats.
    ///  2. Provide a constructor to initialise the colour values.
    ///  3. Implement a method to convert the colour to a string representation.
    ///  4. Overload operators (*, +, -, ==, !=, Equals, GetHashCode) to perform colour operations.
    ///  5. Add static methods to create common colours (e.g. Red, Green, Blue, White, Black).
    ///
    /// Channels are normalised floats rather than bytes because that is what the graphics pipeline
    /// actually consumes: the same convention as UnityEngine.Color and Microsoft.Xna.Framework.Color's
    /// ToVector4. Arithmetic results are clamped to [0, 1] so that adding two bright colours saturates
    /// to white rather than producing values a shader cannot interpret.
    /// </remarks>
    public class ColorRGBA
    {
        #region Static Fields

        private static readonly float DEFAULT_RGB_CHANNEL_VALUE = 1;
        private static readonly float DEFAULT_A_CHANNEL_VALUE = 1;
        private static readonly float MIN_CHANNEL_VALUE = 0;
        private static readonly float MAX_CHANNEL_VALUE = 1;

        #endregion

        #region Instance Fields

        private float r, g, b, a;

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets a new opaque white colour (1, 1, 1, 1).
        /// </summary>
        public static ColorRGBA White { get { return new ColorRGBA(1, 1, 1, 1); } }

        /// <summary>
        /// Gets a new opaque black colour (0, 0, 0, 1).
        /// </summary>
        public static ColorRGBA Black { get { return new ColorRGBA(0, 0, 0, 1); } }

        /// <summary>
        /// Gets a new opaque red colour (1, 0, 0, 1).
        /// </summary>
        public static ColorRGBA Red { get { return new ColorRGBA(1, 0, 0, 1); } }

        /// <summary>
        /// Gets a new opaque green colour (0, 1, 0, 1).
        /// </summary>
        public static ColorRGBA Green { get { return new ColorRGBA(0, 1, 0, 1); } }

        /// <summary>
        /// Gets a new opaque blue colour (0, 0, 1, 1).
        /// </summary>
        public static ColorRGBA Blue { get { return new ColorRGBA(0, 0, 1, 1); } }

        /// <summary>
        /// Gets a new opaque mid-grey colour (0.5, 0.5, 0.5, 1).
        /// </summary>
        /// <remarks>
        /// The f suffix marks each literal as a float; without it the compiler treats 0.5 as a double
        /// and refuses the narrowing conversion.
        /// </remarks>
        public static ColorRGBA Grey { get { return new ColorRGBA(0.5f, 0.5f, 0.5f, 1); } }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets or sets the red channel. Values outside 0 to 1 are replaced with the default channel value.
        /// </summary>
        public float R
        {
            get { return r; }
            set { r = GDMath.ClampToDefault(value, MIN_CHANNEL_VALUE, MAX_CHANNEL_VALUE, DEFAULT_RGB_CHANNEL_VALUE); }
        }

        /// <summary>
        /// Gets or sets the green channel. Values outside 0 to 1 are replaced with the default channel value.
        /// </summary>
        public float G
        {
            get { return g; }
            set { g = GDMath.ClampToDefault(value, MIN_CHANNEL_VALUE, MAX_CHANNEL_VALUE, DEFAULT_RGB_CHANNEL_VALUE); }
        }

        /// <summary>
        /// Gets or sets the blue channel. Values outside 0 to 1 are replaced with the default channel value.
        /// </summary>
        public float B
        {
            get { return b; }
            set { b = GDMath.ClampToDefault(value, MIN_CHANNEL_VALUE, MAX_CHANNEL_VALUE, DEFAULT_RGB_CHANNEL_VALUE); }
        }

        /// <summary>
        /// Gets or sets the alpha (opacity) channel. Values outside 0 to 1 are replaced with the default alpha value.
        /// </summary>
        public float A
        {
            get { return a; }
            set { a = GDMath.ClampToDefault(value, MIN_CHANNEL_VALUE, MAX_CHANNEL_VALUE, DEFAULT_A_CHANNEL_VALUE); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new opaque black colour (0, 0, 0, 1).
        /// </summary>
        public ColorRGBA()
        {
            R = 0;
            G = 0;
            B = 0;
            A = 1;
        }

        /// <summary>
        /// Initialises a new colour from four channel values.
        /// </summary>
        /// <param name="r">The red channel, 0 to 1.</param>
        /// <param name="g">The green channel, 0 to 1.</param>
        /// <param name="b">The blue channel, 0 to 1.</param>
        /// <param name="a">The alpha channel, 0 to 1.</param>
        /// <remarks>
        /// As with <see cref="L8_3DGED_CSharp_Scratch.Player"/>, the constructor assigns through the
        /// properties so that out-of-range arguments are caught at construction time. The original
        /// version wrote straight to the private fields, which meant new ColorRGBA(5, -2, 0, 0) was
        /// accepted without complaint.
        /// </remarks>
        public ColorRGBA(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        #endregion

        #region Housekeeping

        /// <summary>
        /// Returns a human-readable representation of the colour in the form "(r, g, b, a)".
        /// </summary>
        /// <returns>A string containing the four channel values.</returns>
        public override string ToString()
        {
            return $"({r}, {g}, {b}, {a})";
        }

        /// <summary>
        /// Creates a new, independent colour holding the same channel values.
        /// </summary>
        /// <returns>A new <see cref="ColorRGBA"/> with equal channel values.</returns>
        public ColorRGBA DeepCopy()
        {
            return new ColorRGBA(r, g, b, a);
        }

        /// <summary>
        /// Determines whether the supplied object is a <see cref="ColorRGBA"/> with the same four channel values.
        /// </summary>
        /// <param name="obj">The object to compare against this instance.</param>
        /// <returns>True if <paramref name="obj"/> is a colour with equal channels, otherwise false.</returns>
        /// <remarks>
        /// The original implementation had a second return statement after this one that the compiler
        /// flagged as unreachable (CS0162); it has been removed. The comparison is delegated to the
        /// overloaded == operator so that equality is defined in one place only.
        /// Be aware that == on floats is exact: two colours produced by different arithmetic paths can
        /// differ by one unit in the last place and compare unequal. Production code usually tests
        /// Math.Abs(x - y) &lt; epsilon instead.
        /// </remarks>
        public override bool Equals(object obj)
        {
            ColorRGBA other = obj as ColorRGBA;

            if (other == null)
                return false;

            return this == other;
        }

        /// <summary>
        /// Returns a hash code derived from the four channel values.
        /// </summary>
        /// <returns>An integer hash code consistent with <see cref="Equals(object)"/>.</returns>
        /// <remarks>
        /// The multiply-and-add pattern with prime multipliers is order-sensitive, so (1, 0, 0, 1) and
        /// (0, 1, 0, 1) hash differently. A plain XOR of the four channels would collide on those.
        /// </remarks>
        public override int GetHashCode()
        {
            int hashCode = 1091;
            hashCode = hashCode * 1979 + r.GetHashCode();
            hashCode = hashCode * 1033 + g.GetHashCode();
            hashCode = hashCode * 7 + b.GetHashCode();
            hashCode = hashCode * 11 + a.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Adds two colours channel by channel, saturating at 1.
        /// </summary>
        /// <param name="c1">The left-hand colour.</param>
        /// <param name="c2">The right-hand colour.</param>
        /// <returns>A new colour holding the clamped channel-wise sum.</returns>
        /// <remarks>
        /// Additive blending: this is how light sources combine, which is why red plus green gives yellow.
        /// </remarks>
        public static ColorRGBA operator +(ColorRGBA c1, ColorRGBA c2)
        {
            return new ColorRGBA(
                Clamp(c1.r + c2.r),
                Clamp(c1.g + c2.g),
                Clamp(c1.b + c2.b),
                Clamp(c1.a + c2.a));
        }

        /// <summary>
        /// Subtracts the second colour from the first, channel by channel, saturating at 0.
        /// </summary>
        /// <param name="c1">The left-hand colour.</param>
        /// <param name="c2">The right-hand colour.</param>
        /// <returns>A new colour holding the clamped channel-wise difference.</returns>
        public static ColorRGBA operator -(ColorRGBA c1, ColorRGBA c2)
        {
            return new ColorRGBA(
                Clamp(c1.r - c2.r),
                Clamp(c1.g - c2.g),
                Clamp(c1.b - c2.b),
                Clamp(c1.a - c2.a));
        }

        /// <summary>
        /// Multiplies two colours channel by channel (modulation).
        /// </summary>
        /// <param name="c1">The left-hand colour.</param>
        /// <param name="c2">The right-hand colour.</param>
        /// <returns>A new colour holding the channel-wise product.</returns>
        /// <remarks>
        /// This is the operation a shader performs when tinting a texture sample by a material colour.
        /// Because both operands are in [0, 1], the product can never exceed 1, so no clamping is needed.
        /// </remarks>
        public static ColorRGBA operator *(ColorRGBA c1, ColorRGBA c2)
        {
            return new ColorRGBA(
                c1.r * c2.r,
                c1.g * c2.g,
                c1.b * c2.b,
                c1.a * c2.a);
        }

        /// <summary>
        /// Scales every channel of a colour by a scalar, clamping the result to 0 to 1.
        /// </summary>
        /// <param name="c">The colour to scale.</param>
        /// <param name="scalar">The scale factor.</param>
        /// <returns>A new scaled colour.</returns>
        public static ColorRGBA operator *(ColorRGBA c, float scalar)
        {
            return new ColorRGBA(
                Clamp(c.r * scalar),
                Clamp(c.g * scalar),
                Clamp(c.b * scalar),
                Clamp(c.a * scalar));
        }

        /// <summary>
        /// Scales every channel of a colour by a scalar, with the scalar on the left-hand side.
        /// </summary>
        /// <param name="scalar">The scale factor.</param>
        /// <param name="c">The colour to scale.</param>
        /// <returns>A new scaled colour.</returns>
        public static ColorRGBA operator *(float scalar, ColorRGBA c)
        {
            return c * scalar;
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Determines whether two colours hold the same four channel values.
        /// </summary>
        /// <param name="c1">The left-hand colour, which may be null.</param>
        /// <param name="c2">The right-hand colour, which may be null.</param>
        /// <returns>True if both are null, or both are non-null with equal channels.</returns>
        /// <remarks>
        /// ReferenceEquals is used for the null tests because "c1 == null" inside this method would
        /// call the operator recursively until the stack overflows.
        /// </remarks>
        public static bool operator ==(ColorRGBA c1, ColorRGBA c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return true;

            if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
                return false;

            return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b && c1.a == c2.a;
        }

        /// <summary>
        /// Determines whether two colours differ in any channel.
        /// </summary>
        /// <param name="c1">The left-hand colour, which may be null.</param>
        /// <param name="c2">The right-hand colour, which may be null.</param>
        /// <returns>True if the colours are not equal.</returns>
        public static bool operator !=(ColorRGBA c1, ColorRGBA c2)
        {
            return !(c1 == c2);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Converts the colour to its relative luminance using the formula 0.2126R + 0.7152G + 0.0722B.
        /// </summary>
        /// <returns>The perceived brightness of the colour, in the range 0 to 1.</returns>
        /// <remarks>
        /// The weights are unequal because the human eye is far more sensitive to green than to blue.
        /// A naive (R + G + B) / 3 average produces noticeably wrong greys.
        /// Reference: https://en.wikipedia.org/wiki/Relative_luminance
        /// </remarks>
        public float ToLuminance()
        {
            return r * 0.2126f + g * 0.7152f + b * 0.0722f;
        }

        /// <summary>
        /// Converts the colour to its greyscale equivalent by setting all three RGB channels to the luminance value.
        /// </summary>
        /// <returns>A new opaque greyscale <see cref="ColorRGBA"/>.</returns>
        /// <remarks>
        /// Alpha is forced to 1 rather than preserved from the source colour, which is a deliberate
        /// (if debatable) design decision worth discussing in class.
        /// </remarks>
        public ColorRGBA ToGreyscale()
        {
            float luminance = ToLuminance();
            return new ColorRGBA(luminance, luminance, luminance, 1);
        }

        #endregion

        #region Static Methods

        //TODO:Add Lerp
        public static ColorRGBA Lerp(ColorRGBA c1, ColorRGBA c2, float t)
        {
            return new ColorRGBA(
                GDMath.Lerp(c1.r, c2.r, t),
                GDMath.Lerp(c1.g, c2.g, t),
                GDMath.Lerp(c1.b, c2.b, t), 1); 
        }
        public Vector3 ToVector3()
        {
            return new Vector3(r, g, b);
        }
        public ColorRGBA ToColor(Vector3 rgb, float a)
        {
            return new ColorRGBA((float)rgb.X, (float)rgb.Y, (float)rgb.Z, a);
        }


        //NMCG: Dont forget to....

        /// <summary>
        /// Clamps a single channel value to the valid 0 to 1 range.
        /// </summary>
        /// <param name="value">The channel value to clamp.</param>
        /// <returns>The value pulled to the nearest bound if it falls outside the range.</returns>
        /// <remarks>
        /// Private helper used by the arithmetic operators. It calls the three-argument
        /// <see cref="GDMath.Clamp(float, float, float)"/> (a true clamp) rather than the
        /// four-argument overload used by the property setters, which substitutes a default instead.
        /// Subtracting white from black would otherwise produce white.
        /// </remarks>
        private static float Clamp(float value)
        {
            return GDMath.Clamp(value, MIN_CHANNEL_VALUE, MAX_CHANNEL_VALUE);
        }

        #endregion
    }
}
