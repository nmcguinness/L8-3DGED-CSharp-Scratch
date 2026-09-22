using Graphics;

namespace L8_3DGED_CSharp_Scratch
{
    /// <summary>
    /// Represents a player in a game, holding an actor type, a health value and a position in 3D space.
    /// </summary>
    public class Player
    {
        #region Static Fields

        private static readonly string DEFAULT_ACTOR_TYPE = "Default Type";
        private static readonly int DEFAULT_HEALTH = 100;

        #endregion

        #region Instance Fields

        private string actorType;
        private int health;
        private Vector3 position;

        // No isAlive field: liveness is derived from health, so storing it separately would create
        // two sources of truth that can disagree. See the IsAlive property below.

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets or sets the actor type (e.g. "Mage", "Thief"). Null or empty values are replaced with a default.
        /// </summary>
        public string ActorType
        {
            get { return actorType; }
            set { actorType = value != null && value.Length != 0 ? value : DEFAULT_ACTOR_TYPE; }
        }

        /// <summary>
        /// Gets or sets the player's health. Negative values are stored as zero.
        /// </summary>
        /// <remarks>
        /// Clamping in the setter means every route into the field, including the constructor, is
        /// protected. No caller can put the object into a state where health is negative.
        /// </remarks>
        public int Health
        {
            get { return health; }
            set { health = value < 0 ? 0 : value; }
        }

        /// <summary>
        /// Gets or sets the player's position in 3D space. A null value is replaced with the origin.
        /// </summary>
        /// <remarks>
        /// The null guard matters because <see cref="DeepCopy"/> and <see cref="Equals(object)"/> both
        /// dereference this reference; without it, a single null assignment turns those methods into
        /// a NullReferenceException waiting to happen.
        /// </remarks>
        public Vector3 Position
        {
            get { return position; }
            set { position = value ?? new Vector3(); }
        }

        /// <summary>
        /// Gets a value indicating whether the player is still alive, derived from <see cref="Health"/>.
        /// </summary>
        /// <remarks>
        /// A read-only computed property: there is no backing field and nothing to keep in sync.
        /// </remarks>
        public bool IsAlive
        {
            get { return health > 0; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new player with a default actor type, full health and a position at the origin.
        /// </summary>
        /// <remarks>
        /// A parameterless constructor is what serialisation, object initialisers and generic
        /// constraints such as where T : new() rely on, so it is worth keeping even when the
        /// parameterised constructor is the one normally used.
        /// </remarks>
        public Player()
        {
            ActorType = DEFAULT_ACTOR_TYPE;
            Health = DEFAULT_HEALTH;
            Position = new Vector3();
        }

        /// <summary>
        /// Initialises a new player with the supplied actor type, health and position.
        /// </summary>
        /// <param name="actorType">The actor type. Null or empty is replaced with a default.</param>
        /// <param name="health">The starting health. A negative value is stored as zero.</param>
        /// <param name="position">The starting position. Null is replaced with the origin.</param>
        /// <remarks>
        /// Note the assignment through properties (ActorType = ...) rather than fields (actorType = ...):
        /// this is what routes constructor arguments through the same validation as any later assignment.
        /// </remarks>
        public Player(string actorType, int health, Vector3 position)
        {
            ActorType = actorType;
            Health = health;
            Position = position;
        }

        #endregion

        #region Housekeeping

        /// <summary>
        /// Returns a human-readable representation of the player.
        /// </summary>
        /// <returns>A string containing the actor type, health and position.</returns>
        /// <remarks>
        /// The interpolated position calls Vector3.ToString implicitly. Had Vector3 not overridden
        /// ToString, this would print the type name instead of the components.
        /// </remarks>
        public override string ToString()
        {
            return $"Actor: {actorType} (health: {health}, alive: {IsAlive}) at {position}";
        }

        /// <summary>
        /// Returns a reference to this same object, demonstrating a shallow copy.
        /// </summary>
        /// <returns>The current instance, not a new object.</returns>
        /// <remarks>
        /// Every field, including the Vector3 reference, is shared with the "copy" because there is
        /// only one object. Contrast with <see cref="DeepCopy"/>.
        /// </remarks>
        public Player ShallowCopy()
        {
            return this;
        }

        /// <summary>
        /// Creates a new player with the same values and its own independent <see cref="Vector3"/> position.
        /// </summary>
        /// <returns>A new <see cref="Player"/> that shares no mutable objects with this instance.</returns>
        /// <remarks>
        /// The recursive call to position.DeepCopy() is the whole point: copying the reference alone
        /// would leave both players sharing one position object, so moving one would move the other.
        /// A class-by-class deep copy like this is also why the Prototype pattern exists.
        /// </remarks>
        public Player DeepCopy()
        {
            return new Player(actorType, health, position.DeepCopy());
        }

        /// <summary>
        /// Determines whether the supplied object is a <see cref="Player"/> with the same actor type, health and position.
        /// </summary>
        /// <param name="obj">The object to compare against this instance.</param>
        /// <returns>True if the objects are considered equal, otherwise false.</returns>
        /// <remarks>
        /// The "as" cast yields null on type mismatch instead of throwing, unlike the explicit cast
        /// (Player)obj. The ReferenceEquals check is a fast path for the case where both variables
        /// point at the same object. Which fields participate in equality is a design decision:
        /// here all three do, so two players at different positions are not equal.
        /// </remarks>
        public override bool Equals(object obj)
        {
            Player other = obj as Player;

            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return actorType == other.actorType
                && health == other.health
                && position.Equals(other.position);
        }

        /// <summary>
        /// Returns a hash code derived from the fields used by <see cref="Equals(object)"/>.
        /// </summary>
        /// <returns>An integer hash code consistent with <see cref="Equals(object)"/>.</returns>
        /// <remarks>
        /// This override was missing, which produces compiler warning CS0659 and, more importantly,
        /// silently breaks Dictionary and HashSet lookups: two players that compare equal would
        /// otherwise land in different buckets. The rule is absolute - override Equals, override
        /// GetHashCode, and derive both from the same fields.
        /// </remarks>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 31 + (actorType == null ? 0 : actorType.GetHashCode());
            hashCode = hashCode * 31 + health.GetHashCode();
            hashCode = hashCode * 31 + (position == null ? 0 : position.GetHashCode());
            return hashCode;
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Determines whether two players have the same actor type, health and position.
        /// </summary>
        /// <param name="p1">The left-hand player, which may be null.</param>
        /// <param name="p2">The right-hand player, which may be null.</param>
        /// <returns>True if both are null, or both are non-null and hold equal values.</returns>
        /// <remarks>
        /// Overloading == changes the meaning of every existing p1 == p2 in the codebase from
        /// "the same object" to "equal values". That is a deliberate and far-reaching decision:
        /// for an entity type such as a player, two distinct actors that happen to share a name,
        /// a health value and a position are arguably NOT the same player, and reference equality
        /// may well have been the more honest default. Compare with Vector3 and ColorRGBA, where
        /// the object genuinely is nothing more than its values and so value equality is natural.
        ///
        /// ReferenceEquals is used for the null tests because writing "p1 == null" inside this
        /// method would call the operator recursively until the stack overflows. The
        /// ReferenceEquals(p1, p2) test that follows is a fast path for the common case where both
        /// variables point at the same object.
        ///
        /// The position comparison uses Vector3's own == overload, so equality composes: each type
        /// is responsible for defining what equality means for itself.
        /// </remarks>
        public static bool operator ==(Player p1, Player p2)
        {
            if (ReferenceEquals(p1, null) && ReferenceEquals(p2, null))
                return true;

            if (ReferenceEquals(p1, null) || ReferenceEquals(p2, null))
                return false;

            if (ReferenceEquals(p1, p2))
                return true;

            return p1.actorType == p2.actorType
                && p1.health == p2.health
                && p1.position == p2.position;
        }

        /// <summary>
        /// Determines whether two players differ in actor type, health or position.
        /// </summary>
        /// <param name="p1">The left-hand player, which may be null.</param>
        /// <param name="p2">The right-hand player, which may be null.</param>
        /// <returns>True if the players are not equal.</returns>
        /// <remarks>
        /// C# requires == and != to be overloaded as a pair; omitting one is a compile-time error,
        /// not merely a style problem. Defining != in terms of == keeps the two answers consistent.
        /// </remarks>
        public static bool operator !=(Player p1, Player p2)
        {
            return !(p1 == p2);
        }

        #endregion

        #region Class-Specific Methods

        //public void DoDamage(int damage)
        //{
        //    Health -= damage;
        //}

        public int DoDamage(int damage)
        {
            Health -= damage;
            return Health;
        }

        public void DoDamage(int damage, out int newHealth, out bool isAlive)
        {
            //Health -= damage;
            //newHealth = Health;
            //isAlive = Health > 0;

            health -= damage;
            newHealth = health;
            isAlive = health > 0;
        }



        #endregion



    }
}
