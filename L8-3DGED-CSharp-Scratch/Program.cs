using System;
using Engine;
using Graphics;

namespace L8_3DGED_CSharp_Scratch
{
    /// <summary>
    /// Entry point for the console application. Each language feature is demonstrated by its own
    /// self-contained method so that a demo can be run, read or commented out in isolation.
    /// </summary>
    /// <remarks>
    /// Keeping Main to a list of calls is a deliberate teaching pattern: a method should do one thing,
    /// and a 60-line Main that mixes vectors, players and colours makes it impossible to see where one
    /// idea ends and the next begins. The same instinct applies later in Update() in Unity.
    /// </remarks>
    internal class Program
    {
        #region Main

        /// <summary>
        /// Runs each demonstration in turn.
        /// </summary>
        /// <param name="args">Command-line arguments (unused).</param>
        static void Main(string[] args)
        {
            DemoVectorShallowVsDeepCopy();
            DemoVectorOperators();
            DemoVectorEquality();

            DemoPlayerReferenceVsValueEquality();
            DemoPlayerShallowVsDeepCopy();

            DemoColorStaticColorsAndValidation();
            DemoColorArithmetic();
            DemoColorEquality();
            DemoColorLuminanceAndGreyscale();
            DemoColorLerp();

            DemoSwap();
            DemoOut();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void DemoOut()
        {
            Player p1 = new Player("Warrior", 100, new Vector3(0, 0, 0));

            // Damage the player's health
            p1.Health -= 10;

            // Show the player's health before the method call
            Console.WriteLine(p1);

            //Player::DoDamage
            p1.DoDamage(20, out int newHealth, out bool isAlive);
            Console.WriteLine($"new health is {newHealth}");
            Console.WriteLine($"is alive? {isAlive}");

            int newHealthValue;
            bool AmIAlive;
            p1.DoDamage(30, out newHealthValue, out AmIAlive);
            Console.WriteLine($"new health is {newHealthValue}");
            Console.WriteLine($"is alive? {AmIAlive}");
        }

        private static void DemoSwap()
        {
            int x = 5, y = 20;

            GDMath.Swap(ref x, ref y); //Converts value type to reference type using ref keyword

            Console.WriteLine($"x: {x}, y: {y}");
        }

        #endregion

        #region Vector3 Demos

        /// <summary>
        /// Demonstrates the difference between a shallow copy (a second reference to one object) and a
        /// deep copy (a genuinely separate object).
        /// </summary>
        private static void DemoVectorShallowVsDeepCopy()
        {
            PrintHeading("Vector3: shallow vs deep copy");

            Vector3 v1 = new Vector3(1, 2, 3);

            // v2 and v1 now point at the SAME object on the heap
            Vector3 v2 = v1.ShallowCopy();
            v1.X = 100;
            Console.WriteLine($"After v1.X = 100 -> v1: {v1}, v2 (shallow): {v2}");
            Console.WriteLine($"Same object in memory? {ReferenceEquals(v1, v2)}");

            // v3 is a new object holding copies of the values
            Vector3 v3 = v1.DeepCopy();
            v1.Y = 200;
            Console.WriteLine($"After v1.Y = 200 -> v1: {v1}, v3 (deep): {v3}");
            Console.WriteLine($"Same object in memory? {ReferenceEquals(v1, v3)}");
        }

        /// <summary>
        /// Demonstrates the overloaded arithmetic operators, including scalar multiplication from either side.
        /// </summary>
        private static void DemoVectorOperators()
        {
            PrintHeading("Vector3: operator overloading");

            Vector3 v1 = new Vector3(1, 2, 3);
            Vector3 v2 = new Vector3(4, 5, 6);

            Console.WriteLine($"v1 + v2 = {v1 + v2}");
            Console.WriteLine($"v2 - v1 = {v2 - v1}");
            Console.WriteLine($"v1 * v2 = {v1 * v2}   (component-wise, not a dot product)");
            Console.WriteLine($"v1 * 10 = {v1 * 10}");
            Console.WriteLine($"10 * v1 = {10 * v1}   (needs the mirrored overload)");
            Console.WriteLine($"v2 / 2  = {v2 / 2}");

            // Operator precedence is inherited from the built-in operators: * binds tighter than +
            Console.WriteLine($"10 * v1 + v2 * 6 = {10 * v1 + v2 * 6}");

            // The Z setter refuses negative values, so subtraction is not reversible on that channel
            Vector3 below = v1 - v2;
            Console.WriteLine($"v1 - v2 = {below}   (Z clamped to 0 by the property setter)");

            try
            {
                Console.WriteLine(v1 / new Vector3(1, 1, 0));
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine($"Caught as expected: {e.Message}");
            }
        }

        /// <summary>
        /// Demonstrates value equality via the overloaded == operator and the Equals override.
        /// </summary>
        private static void DemoVectorEquality()
        {
            PrintHeading("Vector3: equality");

            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = new Vector3(1, 2, 3);

            Console.WriteLine($"a == b          : {a == b}   (value equality, thanks to the overload)");
            Console.WriteLine($"a.Equals(b)     : {a.Equals(b)}");
            Console.WriteLine($"ReferenceEquals : {ReferenceEquals(a, b)}   (two distinct objects)");
            Console.WriteLine($"Matching hashes : {a.GetHashCode() == b.GetHashCode()}   (required when Equals is true)");
            Console.WriteLine($"a == null       : {a == null}   (null handled without an exception)");
        }

        #endregion

        #region Player Demos

        /// <summary>
        /// Demonstrates that assignment copies a reference, and that Equals compares field values instead.
        /// </summary>
        private static void DemoPlayerReferenceVsValueEquality()
        {
            PrintHeading("Player: reference vs value equality");

            Player p1 = new Player("Mage", 55, new Vector3(3, 2, 1));
            Player p2 = p1;                                              // same object
            Player p3 = new Player("Thief", 44, new Vector3(5, 6, 7));   // different values
            Player p4 = new Player("Mage", 55, new Vector3(3, 2, 1));    // different object, same values

            Console.WriteLine($"p1: {p1}");
            Console.WriteLine($"p2.Equals(p1): {p2.Equals(p1)}   (same object)");
            Console.WriteLine($"p3.Equals(p1): {p3.Equals(p1)}   (different values)");
            Console.WriteLine($"p4.Equals(p1): {p4.Equals(p1)}   (different object, equal values)");
            Console.WriteLine($"p4 == p1     : {p4 == p1}   (== is overloaded, so this is a value test)");
            Console.WriteLine($"p4 != p3     : {p4 != p3}");
            Console.WriteLine($"ReferenceEquals(p4, p1): {ReferenceEquals(p4, p1)}   (still two distinct objects)");
            Console.WriteLine($"p1 == null   : {p1 == null}   (null handled without an exception)");
            Console.WriteLine($"Matching hashes: {p4.GetHashCode() == p1.GetHashCode()}   (required when Equals is true)");

            // Careful: because == now compares values, the only way left to ask "are these the same
            // object?" is ReferenceEquals. Mutating one of two equal players immediately separates
            // them, which is exactly the hazard of value equality on a mutable entity type.
            p4.Health = 10;
            Console.WriteLine($"After p4.Health = 10 -> p4 == p1: {p4 == p1}");

            // Health is clamped by the property setter, and IsAlive is derived rather than stored
            p1.Health = -50;
            Console.WriteLine($"After Health = -50 -> health: {p1.Health}, IsAlive: {p1.IsAlive}");
        }

        /// <summary>
        /// Demonstrates why a deep copy must recurse into referenced objects such as <see cref="Vector3"/>.
        /// </summary>
        private static void DemoPlayerShallowVsDeepCopy()
        {
            PrintHeading("Player: copying an object that contains another object");

            Player original = new Player("Archer", 80, new Vector3(10, 0, 5));
            Player shallow = original.ShallowCopy();
            Player deep = original.DeepCopy();

            // Mutating the position through the original
            original.Position.X = 999;

            Console.WriteLine($"original: {original}");
            Console.WriteLine($"shallow : {shallow}   (shares the same Vector3, so it moved too)");
            Console.WriteLine($"deep    : {deep}   (owns its own Vector3, so it did not)");
        }

        #endregion

        #region ColorRGBA Demos

        /// <summary>
        /// Demonstrates the static colour properties and the channel validation performed by the setters.
        /// </summary>
        private static void DemoColorStaticColorsAndValidation()
        {
            PrintHeading("ColorRGBA: static colours and channel validation");

            Console.WriteLine($"White: {ColorRGBA.White}");
            Console.WriteLine($"Black: {ColorRGBA.Black}");
            Console.WriteLine($"Red  : {ColorRGBA.Red}");
            Console.WriteLine($"Green: {ColorRGBA.Green}");
            Console.WriteLine($"Blue : {ColorRGBA.Blue}");
            Console.WriteLine($"Grey : {ColorRGBA.Grey}");

            // Each access returns a NEW object, so mutating one cannot corrupt the palette for
            // everyone else. This is why they are properties rather than public static fields.
            ColorRGBA myRed = ColorRGBA.Red;
            myRed.G = 1;
            Console.WriteLine($"Mutated copy: {myRed}, but ColorRGBA.Red is still {ColorRGBA.Red}");

            // Out-of-range channels are replaced with the default channel value (1), NOT clamped to
            // the nearest bound. Worth pausing on: GDMath.Clamp(value, min, max, defaultValue) is a
            // validate-or-substitute, so 1.5 and -0.5 both become 1.
            ColorRGBA invalid = new ColorRGBA(1.5f, 0.5f, -0.5f, 1);
            Console.WriteLine($"new ColorRGBA(1.5f, 0.5f, -0.5f, 1) -> {invalid}");
        }

        /// <summary>
        /// Demonstrates the overloaded arithmetic operators and the saturation behaviour of each.
        /// </summary>
        private static void DemoColorArithmetic()
        {
            PrintHeading("ColorRGBA: arithmetic operators");

            ColorRGBA red = ColorRGBA.Red;
            ColorRGBA green = ColorRGBA.Green;
            ColorRGBA blue = ColorRGBA.Blue;
            ColorRGBA grey = ColorRGBA.Grey;

            Console.WriteLine($"Red + Green      = {red + green}   (additive light: yellow)");
            Console.WriteLine($"Red + Green + Blue = {red + green + blue}   (saturates to white)");
            Console.WriteLine($"White - Red      = {ColorRGBA.White - red}   (cyan; note alpha went to 0)");
            Console.WriteLine($"Red * Grey       = {red * grey}   (modulation: a tinted, darker red)");
            Console.WriteLine($"Grey * 2f        = {grey * 2f}   (scalar scale, clamped at 1)");
            Console.WriteLine($"2f * Grey        = {2f * grey}   (mirrored overload)");
            Console.WriteLine($"Red * 0.25f      = {red * 0.25f}");

            // Discussion point: every operator here treats alpha as just another channel, so
            // White - Red comes back fully transparent and Red * 0.25f comes back 75% transparent
            // as well as darker. Most engines deliberately special-case alpha. Ask the class which
            // behaviour they would want, then have them change the operators to match.
        }

        /// <summary>
        /// Demonstrates value equality for colours and the Equals/GetHashCode contract.
        /// </summary>
        private static void DemoColorEquality()
        {
            PrintHeading("ColorRGBA: equality");

            ColorRGBA c1 = new ColorRGBA(1, 0, 0, 1);
            ColorRGBA c2 = ColorRGBA.Red;
            ColorRGBA c3 = ColorRGBA.Blue;

            Console.WriteLine($"c1 == c2            : {c1 == c2}");
            Console.WriteLine($"c1 != c3            : {c1 != c3}");
            Console.WriteLine($"c1.Equals(c2)       : {c1.Equals(c2)}");
            Console.WriteLine($"c1.Equals(\"red\")    : {c1.Equals("red")}   (wrong type, handled by the 'as' cast)");
            Console.WriteLine($"ReferenceEquals     : {ReferenceEquals(c1, c2)}   (equal values, different objects)");
            Console.WriteLine($"Matching hashes     : {c1.GetHashCode() == c2.GetHashCode()}");
            Console.WriteLine($"Red hash vs Blue    : {ColorRGBA.Red.GetHashCode()} vs {ColorRGBA.Blue.GetHashCode()}");
            Console.WriteLine($"c1 == null          : {c1 == null}   (null handled without an exception)");

            // Caution: these comparisons are exact float tests. Colours arrived at through different
            // arithmetic can differ in the last bit and compare unequal, which is why production code
            // usually tests Math.Abs(x - y) < epsilon rather than ==.

            // A copy is equal by value but is a separate object
            ColorRGBA copy = c1.DeepCopy();
            Console.WriteLine($"DeepCopy equal? {copy == c1}, same object? {ReferenceEquals(copy, c1)}");
        }

        /// <summary>
        /// Demonstrates perceptual luminance and greyscale conversion.
        /// </summary>
        private static void DemoColorLuminanceAndGreyscale()
        {
            PrintHeading("ColorRGBA: luminance and greyscale");

            ColorRGBA[] palette = { ColorRGBA.Red, ColorRGBA.Green, ColorRGBA.Blue, ColorRGBA.Grey, ColorRGBA.White };

            foreach (ColorRGBA color in palette)
                Console.WriteLine($"{color} -> luminance {color.ToLuminance():F4} -> greyscale {color.ToGreyscale()}");

            // Note how green carries far more perceived brightness than blue, even though both are
            // a single channel at full strength. That is the weighting in ToLuminance doing its job.
        }

        /// <summary>
        /// Demonstrates linear interpolation between two colours, the basis of every fade and tint.
        /// </summary>
        private static void DemoColorLerp()
        {
            PrintHeading("ColorRGBA: linear interpolation");

            ColorRGBA start = ColorRGBA.Red;
            ColorRGBA end = ColorRGBA.Blue;

            //TODO: Implement linear interpolation demonstration
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Writes a visually distinct heading to the console to separate one demonstration from the next.
        /// </summary>
        /// <param name="heading">The text to display.</param>
        private static void PrintHeading(string heading)
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 70));
            Console.WriteLine(heading.ToUpper());
            Console.WriteLine(new string('-', 70));
        }

        #endregion
    }
}