
namespace Graphics
{
    /*
     1. Store colors for 4 channels (Red, Green, Blue, Alpha) as floats.
     2. Provide a constructor to initialize the color values.
     3. Implement a method to convert the color to a string representation.
     4. Overload operator (i.e., *, +, -, ==, !=, Equals, GetHashCode) to perform color operations.
     5. Add static methods to create common colors (e.g., Red, Green, Blue, White, Black).
     */
    public class ColorRGBA
    {
        #region Static Fields
        private static readonly float DEFAULT_RGB_CHANNEL_VALUE = 1;
        private static readonly float DEFAULT_A_CHANNEL_VALUE = 1;
        #endregion

        #region Instance Fields
        private float r, g, b, a;
        #endregion

        #region Static Properties
        public static ColorRGBA White = new ColorRGBA(1, 1, 1, 1);
        public static ColorRGBA Black = new ColorRGBA(0, 0, 0, 1);
        public static ColorRGBA Red = new ColorRGBA(1, 0, 0, 1);
        public static ColorRGBA Green = new ColorRGBA(0, 1, 0, 1);
        public static ColorRGBA Blue = new ColorRGBA(0, 0, 1, 1);
        public static ColorRGBA Grey = new ColorRGBA(0.5, 0.5, 0.5, 1); 
        #endregion



        #region Instance Properties
        public float R
        {
            get
            {
                return r;
            }
            set
            {
                r = Engine.GDMath.Clamp(value, 0, 1, DEFAULT_RGB_CHANNEL_VALUE);
            }
        }

        public float G
        {
            get
            {
                return g;
            }
            set
            {
                g = Engine.GDMath.Clamp(value, 0, 1, DEFAULT_RGB_CHANNEL_VALUE);
            }
        }

        public float B
        {
            get
            {
                return b;
            }
            set
            {
                b = Engine.GDMath.Clamp(value, 0, 1, DEFAULT_RGB_CHANNEL_VALUE);
            }
        }

        public float A
        {
            get
            {
                return a;
            }
            set
            {
                a = Engine.GDMath.Clamp(value, 0, 1, DEFAULT_A_CHANNEL_VALUE);
            }
        } 
        #endregion
    }
}
