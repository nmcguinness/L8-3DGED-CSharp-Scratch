using System;

namespace L8_3DGED_CSharp_Scratch
{
    /// <summary>
    /// Represents a point or direction in 3D space using double-precision X, Y and Z components.
    /// </summary>
    /// <remarks>
    /// Used to demonstrate four ideas that recur throughout Unity and MonoGame development:
    /// encapsulation of fields behind validating properties, the difference between a shallow
    /// and a deep copy of a reference type, operator overloading, and the contract that binds
    /// <see cref="Equals(object)"/>, <see cref="GetHashCode"/> and the equality operators.
    /// Note that UnityEngine.Vector3 is a struct (a value type) with float components, so it
    /// copies by value on assignment. This class is a reference type, which is precisely why
    /// the shallow/deep copy distinction below matters.
    /// </remarks>
    public class Vector3
    {
        #region Instance Fields

        private double x, y, z;

        #endregion

        #region Instance Properties

        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Gets or sets the Z component of the vector. Negative values are rejected and stored as zero.
        /// </summary>
        /// <remarks>
        /// The validation here is purely a teaching device to show that a property setter can do more
        /// than assign; a real 3D vector would of course allow a negative Z. Be aware that it makes
        /// the type asymmetric: v.Z = -5 does not round-trip, so operator results involving Z can
        /// surprise you (see the operator region below).
        /// </remarks>
        public double Z
        {
            get { return z; }
            set { z = value < 0 ? 0 : value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new vector at the origin (0, 0, 0).
        /// </summary>
        public Vector3()
        {
            X = Y = Z = 0;
        }

        /// <summary>
        /// Initialises a new vector with the supplied components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component. A negative value is stored as zero (see <see cref="Z"/>).</param>
        /// <remarks>
        /// The constructor assigns through the public properties rather than directly to the private
        /// fields so that the validation in <see cref="Z"/> is applied at construction time as well as
        /// on later assignment. Writing to the fields directly is a common source of objects that are
        /// born in a state their own setters would have refused.
        /// </remarks>
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Housekeeping

        /// <summary>
        /// Returns a human-readable representation of the vector in the form "(x, y, z)".
        /// </summary>
        /// <returns>A string containing the three components.</returns>
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        /// <summary>
        /// Returns a reference to this same object, demonstrating a shallow copy.
        /// </summary>
        /// <returns>The current instance, not a new object.</returns>
        /// <remarks>
        /// Both variables end up pointing at one object on the heap, so a change made through either
        /// variable is visible through the other. This is exactly what plain assignment (v2 = v1)
        /// already does for a reference type; the method exists only to make the behaviour explicit.
        /// </remarks>
        public Vector3 ShallowCopy()
        {
            return this;
        }

        /// <summary>
        /// Creates a new, independent vector holding the same component values, demonstrating a deep copy.
        /// </summary>
        /// <returns>A new <see cref="Vector3"/> with equal component values.</returns>
        /// <remarks>
        /// Because <see cref="Vector3"/> contains only value-type fields, copying the components is
        /// sufficient. Once a class holds references to other objects, a deep copy must recursively
        /// copy those too (see <see cref="Player.DeepCopy"/>).
        /// </remarks>
        public Vector3 DeepCopy()
        {
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Determines whether the supplied object is a <see cref="Vector3"/> with the same component values.
        /// </summary>
        /// <param name="obj">The object to compare against this instance.</param>
        /// <returns>True if <paramref name="obj"/> is a Vector3 with equal components, otherwise false.</returns>
        /// <remarks>
        /// The "as" cast returns null on failure rather than throwing, which lets us test the type and
        /// convert it in a single step. The comparison itself is delegated to the overloaded ==
        /// operator so that value equality is defined in exactly one place.
        /// </remarks>
        public override bool Equals(object obj)
        {
            Vector3 other = obj as Vector3;

            if (other == null)
                return false;

            return this == other;
        }

        /// <summary>
        /// Returns a hash code derived from the three components.
        /// </summary>
        /// <returns>An integer hash code consistent with <see cref="Equals(object)"/>.</returns>
        /// <remarks>
        /// Overriding Equals without overriding GetHashCode breaks Dictionary and HashSet lookups, so
        /// the two are always overridden together. Note the standing hazard of hashing mutable state:
        /// mutating X, Y or Z after using a vector as a dictionary key leaves that entry unreachable.
        /// XOR is used here for simplicity; the multiply-and-add pattern in
        /// <see cref="Graphics.ColorRGBA.GetHashCode"/> distributes more evenly because it is order-sensitive.
        /// </remarks>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Adds two vectors component-wise.
        /// </summary>
        /// <param name="v1">The left-hand vector.</param>
        /// <param name="v2">The right-hand vector.</param>
        /// <returns>A new vector holding the component-wise sum.</returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Subtracts the second vector from the first, component-wise.
        /// </summary>
        /// <param name="v1">The left-hand vector.</param>
        /// <param name="v2">The right-hand vector.</param>
        /// <returns>A new vector holding the component-wise difference.</returns>
        /// <remarks>
        /// Remember that a negative Z result is forced to zero by the <see cref="Z"/> setter, so
        /// (a - b) + b does not necessarily return a. A useful worked example for students of why
        /// validation belongs where the domain actually requires it.
        /// </remarks>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Multiplies two vectors component-wise (the Hadamard product, not the dot or cross product).
        /// </summary>
        /// <param name="v1">The left-hand vector.</param>
        /// <param name="v2">The right-hand vector.</param>
        /// <returns>A new vector holding the component-wise product.</returns>
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        /// <summary>
        /// Scales a vector by a scalar.
        /// </summary>
        /// <param name="v">The vector to scale.</param>
        /// <param name="scalar">The scale factor.</param>
        /// <returns>A new scaled vector.</returns>
        public static Vector3 operator *(Vector3 v, double scalar)
        {
            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        /// <summary>
        /// Scales a vector by a scalar, with the scalar on the left-hand side.
        /// </summary>
        /// <param name="scalar">The scale factor.</param>
        /// <param name="v">The vector to scale.</param>
        /// <returns>A new scaled vector.</returns>
        /// <remarks>
        /// Operator overloads are matched on the static types of both operands, so this mirror overload
        /// is what makes the expression "10 * v" legal as well as "v * 10".
        /// </remarks>
        public static Vector3 operator *(double scalar, Vector3 v)
        {
            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        /// <summary>
        /// Divides two vectors component-wise.
        /// </summary>
        /// <param name="v1">The dividend vector.</param>
        /// <param name="v2">The divisor vector.</param>
        /// <returns>A new vector holding the component-wise quotient.</returns>
        /// <exception cref="DivideByZeroException">Thrown when any component of <paramref name="v2"/> is zero.</exception>
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            if (v2.X == 0 || v2.Y == 0 || v2.Z == 0)
                throw new DivideByZeroException("Cannot divide by zero");

            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        /// <summary>
        /// Divides a vector by a scalar.
        /// </summary>
        /// <param name="v1">The vector to divide.</param>
        /// <param name="scalar">The divisor.</param>
        /// <returns>A new vector holding the divided components.</returns>
        /// <exception cref="DivideByZeroException">Thrown when <paramref name="scalar"/> is zero.</exception>
        /// <remarks>
        /// The parameter is a double rather than a float so that this overload is consistent with the
        /// multiplication overloads above; a float or int argument still binds here through implicit conversion.
        /// </remarks>
        public static Vector3 operator /(Vector3 v1, double scalar)
        {
            if (scalar == 0)
                throw new DivideByZeroException("Cannot divide by zero");

            return new Vector3(v1.X / scalar, v1.Y / scalar, v1.Z / scalar);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Determines whether two vectors hold the same component values.
        /// </summary>
        /// <param name="v1">The left-hand vector, which may be null.</param>
        /// <param name="v2">The right-hand vector, which may be null.</param>
        /// <returns>True if both are null, or both are non-null with equal components.</returns>
        /// <remarks>
        /// ReferenceEquals is used for the null tests rather than == because writing "v1 == null" inside
        /// this method would call the operator recursively and overflow the stack. This is the classic
        /// trap when overloading == on a reference type.
        /// </remarks>
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            if (ReferenceEquals(v1, null) && ReferenceEquals(v2, null))
                return true;

            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
                return false;

            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        /// <summary>
        /// Determines whether two vectors differ in any component.
        /// </summary>
        /// <param name="v1">The left-hand vector, which may be null.</param>
        /// <param name="v2">The right-hand vector, which may be null.</param>
        /// <returns>True if the vectors are not equal.</returns>
        /// <remarks>
        /// C# requires == and != to be overloaded as a pair. Defining != in terms of == keeps the two
        /// answers from drifting apart as the class evolves.
        /// </remarks>
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        #endregion
    }
}
