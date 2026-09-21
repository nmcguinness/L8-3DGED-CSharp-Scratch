
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
        private static readonly float DEFAULT_RGB_CHANNEL_VALUE = 1;

        private float r, g, b, a;

        public float R
        {
            get
            {
                return r;
            }
            set
            {
                r = value >= 0 && value <= 1 ? value : DEFAULT_RGB_CHANNEL_VALUE;
            }
        }
    }
}
