namespace Engine
{
    public class GDMath
    {
        public static float Clamp(float value, float min, float max, float defaultValue)
        {
            return value >= min && value <= max ? value : defaultValue;
        }
    }
}
