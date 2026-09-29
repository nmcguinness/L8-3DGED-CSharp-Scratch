using Engine;
using Graphics;
using L8_3DGED_CSharp_Scratch.Demos;
using System;
using System.Collections.Generic;

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
    public class Program
    {
        #region Fields

        /// <summary>
        /// A pickup delegate holds address of 1 or more methods/functions that take two parameters:
        /// an integer amount and a PickupType enum. It is used to notify multiple listeners when a
        /// pickup is collected.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        public delegate void PickupHandler(int amount, PickupType type);

        /// <summary>
        /// The AudioManager instance used in the delegate demonstration.
        /// </summary>
        private static AudioManager _audioManager;

        /// <summary>
        /// The EnemyManager instance used in the event demonstration.
        /// </summary>
        private static EnemyManager _enemyManager;

        #endregion

        #region Main

        /// <summary>
        /// Runs each demonstration in turn.
        /// </summary>
        /// <param name="args">Command-line arguments (unused).</param>
        static void Main(string[] args)
        {
            Program app = new Program();
            app.Run();
        }

        /// <summary>
        /// Runs all demonstration methods in a logical sequence.
        /// </summary>
        private void Run()
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

            DemoSwap();
            DemoOut();
            DemoInterface();

            DemoInterfaceAndStrategy();
            DemoColorLerp();

            DemoAbstractClasses();

            DemoManagerSetup();
            DemoDelegate();
            DemoEvent();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #endregion

        #region Event and Delegate Demos

        /// <summary>
        /// Creates and configures the manager objects used by the delegate and event demonstrations.
        /// </summary>
        private void DemoManagerSetup()
        {
            _audioManager = new AudioManager();
            _audioManager.AddPickupCue(PickupType.Health, "health.wav");
            _audioManager.AddPickupCue(PickupType.Ammo, "ammo.wav");
            _audioManager.AddPickupCue(PickupType.Shield, "shield.wav");

            _enemyManager = new EnemyManager(Enemy.Barbarian);
        }

        /// <summary>
        /// Demonstrates creating, combining, invoking and removing methods from a multicast delegate.
        /// </summary>
        private void DemoDelegate()
        {
            PrintHeading("Delegate: Registering event handlers");

            //create an entity that responds to pickups, e.g. health bar, audio manager, big boss
            BigBoss sid = new BigBoss();

            //create an entity that responds to pickups, e.g. health bar, audio manager, big boss
            HealthBar hb = new HealthBar(5);

            //create a delegate instance and add methods to it e.g. Refresh, PlayPickupSound, Notify
            PickupHandler handler = hb.Refresh;
            handler += sid.Notify;
            handler += _audioManager.PlayPickupCue;

            //called when a pickup is collected, e.g. in Player::CollectPickup
            handler(3, PickupType.Ammo);

            //remove the boss some time later because he is dead or out of range
            handler -= sid.Notify;

            //pickup collected again, but the boss is no longer listening
            handler(3, PickupType.Health);
        }

        /// <summary>
        /// Demonstrates subscribing to and unsubscribing from a C# event.
        /// </summary>
        private void DemoEvent()
        {
            PrintHeading("Event: Registering event handlers");

            //create a difficulty level object with min and max values
            DifficultyLevel dl = new DifficultyLevel(1, 5);

            //register the enemy manager's method to respond to the event
            dl.OnDifficultyChanged += _enemyManager.HandleDifficultyChanged;

            //player finds a level too hard, so decrease the difficulty
            dl.SetDifficult(-1);

            //deregister the enemy manager's method to stop responding to the event
            dl.OnDifficultyChanged -= _enemyManager.HandleDifficultyChanged;
        }

        #endregion

        #region Inheritance and Interface Demos

        /// <summary>
        /// Demonstrates polymorphism using a collection of objects derived from an abstract base class.
        /// </summary>
        private void DemoAbstractClasses()
        {
            PrintHeading("PickupBase: Using abstract classes");

            List<PickupBase> pickups = new List<PickupBase>
            {
                new HealthPickup(25),
                new AmmoPickup(12)
            };

            //List<PickupBase> pickups = new List<PickupBase>();
            //pickups.Add(new HealthPickup(25));
            //pickups.Add(new AmmoPickup(12));

            foreach (PickupBase pickup in pickups)
            {
                //ERROR: is this plasma rifle ammo, then delete?
                pickup.Collect();
            }
        }

        /// <summary>
        /// Demonstrates interface-based polymorphism with objects that implement IDamageable.
        /// </summary>
        private void DemoInterface()
        {
            PrintHeading("IDamageable: adding damageable objects");

            Explosion e = new Explosion();

            e.Add(new Enemy(100, 0, false));
            e.Add(new Enemy(50, 1, true));
            e.Add(new Barrel(30));

            // Polymorphism: an interface reference can point at any object that implements that interface
            IDamageable d1 = new Enemy(40, 0, false);
            e.Add(d1);

            e.Detonate(20);

            Console.WriteLine("Health of each target after the explosion:");
            for (int i = 0; i < e.Targets.Count; i++)
                Console.WriteLine(e.Targets[i]);
        }

        /// <summary>
        /// Demonstrates using an interface to implement and inject an interchangeable strategy.
        /// </summary>
        private void DemoInterfaceAndStrategy()
        {
            PrintHeading("Interface and Strategy: Using interface classes");

            List<Player> pList = new List<Player>();
            pList.Add(new Player("thief", 55, new Vector3(1, 5, 10)));
            pList.Add(new Player("mage", 99, new Vector3(2, 4, 6)));

            IAttackStrategy attackStrategy
                = new ActorProximityStrategy("mage", 20);

            // Quick and dirty test of the strategy
            Console.WriteLine(attackStrategy.FilterBy(pList));

            Turret mainGateTurret = new Turret(
                new Vector3(10, 10, 5),
                attackStrategy);

            mainGateTurret.Attack(pList);
        }

        #endregion

        #region Parameter Passing Demos

        /// <summary>
        /// Demonstrates using ref parameters to modify caller variables inside a method.
        /// </summary>
        private void DemoSwap()
        {
            PrintHeading("Swap: A ref demo");

            int x = 5, y = 20;

            GDMath.Swap(ref x, ref y);

            Console.WriteLine($"x: {x}, y: {y}");
        }

        /// <summary>
        /// Demonstrates using out parameters to return multiple values from a method.
        /// </summary>
        private void DemoOut()
        {
            PrintHeading("Classes: creating objects with 'out' parameters");

            Player p1 = new Player("Warrior", 100, new Vector3(0, 0, 0));

            // Damage the player's health
            p1.Health -= 10;

            // Show the player's health before the method call
            Console.WriteLine(p1);

            p1.DoDamage(20, out int newHealth, out bool isAlive);
            Console.WriteLine($"new health is {newHealth}");
            Console.WriteLine($"is alive? {isAlive}");

            int newHealthValue;
            bool AmIAlive;

            p1.DoDamage(30, out newHealthValue, out AmIAlive);

            Console.WriteLine($"new health is {newHealthValue}");
            Console.WriteLine($"is alive? {AmIAlive}");
        }

        #endregion

        #region Vector3 Demos

        /// <summary>
        /// Demonstrates the difference between a shallow copy (a second reference to one object)
        /// and a deep copy (a genuinely separate object).
        /// </summary>
        private void DemoVectorShallowVsDeepCopy()
        {
            PrintHeading("Vector3: shallow vs deep copy");

            Vector3 v1 = new Vector3(1, 2, 3);

            // v2 and v1 now point at the SAME object on the heap
            Vector3 v2 = v1.ShallowCopy();

            v1.X = 100;

            Console.WriteLine(
                $"After v1.X = 100 -> v1: {v1}, v2 (shallow): {v2}");

            Console.WriteLine(
                $"Same object in memory? {ReferenceEquals(v1, v2)}");

            // v3 is a new object holding copies of the values
            Vector3 v3 = v1.DeepCopy();

            v1.Y = 200;

            Console.WriteLine(
                $"After v1.Y = 200 -> v1: {v1}, v3 (deep): {v3}");

            Console.WriteLine(
                $"Same object in memory? {ReferenceEquals(v1, v3)}");
        }

        /// <summary>
        /// Demonstrates the overloaded arithmetic operators,
        /// including scalar multiplication from either side.
        /// </summary>
        private void DemoVectorOperators()
        {
            PrintHeading("Vector3: operator overloading");

            Vector3 v1 = new Vector3(1, 2, 3);
            Vector3 v2 = new Vector3(4, 5, 6);

            Console.WriteLine($"v1 + v2 = {v1 + v2}");
            Console.WriteLine($"v2 - v1 = {v2 - v1}");

            Console.WriteLine(
                $"v1 * v2 = {v1 * v2}   (component-wise, not a dot product)");

            Console.WriteLine($"v1 * 10 = {v1 * 10}");

            Console.WriteLine(
                $"10 * v1 = {10 * v1}   (needs the mirrored overload)");

            Console.WriteLine($"v2 / 2  = {v2 / 2}");

            Console.WriteLine(
                $"10 * v1 + v2 * 6 = {10 * v1 + v2 * 6}");

            Vector3 below = v1 - v2;

            Console.WriteLine(
                $"v1 - v2 = {below}   (Z clamped to 0 by the property setter)");

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
        /// Demonstrates value equality via the overloaded == operator
        /// and the Equals override.
        /// </summary>
        private void DemoVectorEquality()
        {
            PrintHeading("Vector3: equality");

            Vector3 a = new Vector3(1, 2, 3);
            Vector3 b = new Vector3(1, 2, 3);

            Console.WriteLine(
                $"a == b          : {a == b}   (value equality, thanks to the overload)");

            Console.WriteLine($"a.Equals(b)     : {a.Equals(b)}");

            Console.WriteLine(
                $"ReferenceEquals : {ReferenceEquals(a, b)}   (two distinct objects)");

            Console.WriteLine(
                $"Matching hashes : {a.GetHashCode() == b.GetHashCode()}");

            Console.WriteLine(
                $"a == null       : {a == null}");
        }

        #endregion

        #region Player Demos

        /// <summary>
        /// Demonstrates that assignment copies a reference,
        /// and that Equals compares field values instead.
        /// </summary>
        private void DemoPlayerReferenceVsValueEquality()
        {
            PrintHeading("Player: reference vs value equality");

            Player p1 =
                new Player("Mage", 55, new Vector3(3, 2, 1));

            Player p2 = p1;

            Player p3 =
                new Player("Thief", 44, new Vector3(5, 6, 7));

            Player p4 =
                new Player("Mage", 55, new Vector3(3, 2, 1));

            Console.WriteLine($"p1: {p1}");
            Console.WriteLine($"p2.Equals(p1): {p2.Equals(p1)}");
            Console.WriteLine($"p3.Equals(p1): {p3.Equals(p1)}");
            Console.WriteLine($"p4.Equals(p1): {p4.Equals(p1)}");
            Console.WriteLine($"p4 == p1     : {p4 == p1}");
            Console.WriteLine($"p4 != p3     : {p4 != p3}");

            Console.WriteLine(
                $"ReferenceEquals(p4, p1): {ReferenceEquals(p4, p1)}");

            Console.WriteLine($"p1 == null   : {p1 == null}");

            Console.WriteLine(
                $"Matching hashes: {p4.GetHashCode() == p1.GetHashCode()}");

            p4.Health = 10;

            Console.WriteLine(
                $"After p4.Health = 10 -> p4 == p1: {p4 == p1}");

            p1.Health = -50;

            Console.WriteLine(
                $"After Health = -50 -> health: {p1.Health}, IsAlive: {p1.IsAlive}");
        }

        /// <summary>
        /// Demonstrates why a deep copy must recurse into referenced
        /// objects such as Vector3.
        /// </summary>
        private void DemoPlayerShallowVsDeepCopy()
        {
            PrintHeading(
                "Player: copying an object that contains another object");

            Player original =
                new Player("Archer", 80, new Vector3(10, 0, 5));

            Player shallow = original.ShallowCopy();
            Player deep = original.DeepCopy();

            original.Position.X = 999;

            Console.WriteLine($"original: {original}");

            Console.WriteLine(
                $"shallow : {shallow}   (shares the same Vector3, so it moved too)");

            Console.WriteLine(
                $"deep    : {deep}   (owns its own Vector3, so it did not)");
        }

        #endregion

        #region ColorRGBA Demos

        /// <summary>
        /// Demonstrates the static colour properties and the
        /// channel validation performed by the setters.
        /// </summary>
        private void DemoColorStaticColorsAndValidation()
        {
            PrintHeading(
                "ColorRGBA: static colours and channel validation");

            Console.WriteLine($"White: {ColorRGBA.White}");
            Console.WriteLine($"Black: {ColorRGBA.Black}");
            Console.WriteLine($"Red  : {ColorRGBA.Red}");
            Console.WriteLine($"Green: {ColorRGBA.Green}");
            Console.WriteLine($"Blue : {ColorRGBA.Blue}");
            Console.WriteLine($"Grey : {ColorRGBA.Grey}");

            ColorRGBA myRed = ColorRGBA.Red;
            myRed.G = 1;

            Console.WriteLine(
                $"Mutated copy: {myRed}, but ColorRGBA.Red is still {ColorRGBA.Red}");

            ColorRGBA invalid =
                new ColorRGBA(1.5f, 0.5f, -0.5f, 1);

            Console.WriteLine(
                $"new ColorRGBA(1.5f, 0.5f, -0.5f, 1) -> {invalid}");
        }

        /// <summary>
        /// Demonstrates the overloaded arithmetic operators
        /// and the saturation behaviour of each.
        /// </summary>
        private void DemoColorArithmetic()
        {
            PrintHeading("ColorRGBA: arithmetic operators");

            ColorRGBA red = ColorRGBA.Red;
            ColorRGBA green = ColorRGBA.Green;
            ColorRGBA blue = ColorRGBA.Blue;
            ColorRGBA grey = ColorRGBA.Grey;

            Console.WriteLine(
                $"Red + Green = {red + green}");

            Console.WriteLine(
                $"Red + Green + Blue = {red + green + blue}");

            Console.WriteLine(
                $"White - Red = {ColorRGBA.White - red}");

            Console.WriteLine(
                $"Red * Grey = {red * grey}");

            Console.WriteLine(
                $"Grey * 2f = {grey * 2f}");

            Console.WriteLine(
                $"2f * Grey = {2f * grey}");

            Console.WriteLine(
                $"Red * 0.25f = {red * 0.25f}");
        }

        /// <summary>
        /// Demonstrates value equality for colours and
        /// the Equals/GetHashCode contract.
        /// </summary>
        private void DemoColorEquality()
        {
            PrintHeading("ColorRGBA: equality");

            ColorRGBA c1 = new ColorRGBA(1, 0, 0, 1);
            ColorRGBA c2 = ColorRGBA.Red;
            ColorRGBA c3 = ColorRGBA.Blue;

            Console.WriteLine($"c1 == c2       : {c1 == c2}");
            Console.WriteLine($"c1 != c3       : {c1 != c3}");
            Console.WriteLine($"c1.Equals(c2)  : {c1.Equals(c2)}");

            Console.WriteLine(
                $"ReferenceEquals: {ReferenceEquals(c1, c2)}");

            Console.WriteLine(
                $"Matching hashes: {c1.GetHashCode() == c2.GetHashCode()}");

            Console.WriteLine(
                $"Red hash vs Blue: {ColorRGBA.Red.GetHashCode()} vs {ColorRGBA.Blue.GetHashCode()}");

            Console.WriteLine(
                $"c1 == null: {c1 == null}");

            ColorRGBA copy = c1.DeepCopy();

            Console.WriteLine(
                $"DeepCopy equal? {copy == c1}, same object? {ReferenceEquals(copy, c1)}");
        }

        /// <summary>
        /// Demonstrates perceptual luminance and greyscale conversion.
        /// </summary>
        private void DemoColorLuminanceAndGreyscale()
        {
            PrintHeading(
                "ColorRGBA: luminance and greyscale");

            ColorRGBA[] palette =
            {
                ColorRGBA.Red,
                ColorRGBA.Green,
                ColorRGBA.Blue,
                ColorRGBA.Grey,
                ColorRGBA.White
            };

            foreach (ColorRGBA color in palette)
            {
                Console.WriteLine(
                    $"{color} -> luminance {color.ToLuminance():F4} -> greyscale {color.ToGreyscale()}");
            }
        }

        /// <summary>
        /// Demonstrates linear interpolation between two colours,
        /// the basis of fades and colour transitions.
        /// </summary>
        private void DemoColorLerp()
        {
            PrintHeading(
                "ColorRGBA: linear interpolation");

            ColorRGBA start = ColorRGBA.Red;
            ColorRGBA end = ColorRGBA.Blue;

            for (int i = 0; i <= 10; i++)
            {
                float t = i / 10f;

                ColorRGBA lerpedColor =
                    ColorRGBA.Lerp(start, end, t);

                Console.WriteLine(
                    $"t={t:F1}: {lerpedColor}");
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Writes a visually distinct heading to the console
        /// to separate one demonstration from the next.
        /// </summary>
        /// <param name="heading">The text to display.</param>
        private void PrintHeading(string heading)
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 70));
            Console.WriteLine(heading.ToUpper());
            Console.WriteLine(new string('-', 70));
        }

        #endregion
    }
}