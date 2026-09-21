using System;

namespace L8_3DGED_CSharp_Scratch
{

    /// <summary>
    /// A simple class representing a 3D vector with X, Y, and Z coordinates. Provides a demo of shallow and deep copy methods, as well as input validation for the Z coordinate.
    /// </summary>
    public class Vector3
    {
        private double x, y, z;

        #region Properties
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public double Z
        {
            get { return z; }
            set { z = value < 0 ? 0 : value; } //as a demo, lets add some input validation to make sure the z value is not negative
        }
        #endregion

        #region Constructors
        public Vector3()
        {
            x = y = z = 0;
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        #endregion

        #region Housekeeping
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        public Vector3 ShallowCopy()
        {
            return this; //the address of the current object is returned, so any changes made to the copy will affect the original object
        }

        public Vector3 DeepCopy()
        {
            return new Vector3(x, y, z); //a new object is created with the same values, so changes made to the copy will not affect the original object
        }
        #endregion

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }
        public static Vector3 operator /(Vector3 v1, Vector3 v2)
        {
            if (v2.X == 0 || v2.Y == 0 || v2.Z == 0)
                throw new DivideByZeroException("Cannot divide by zero");

            return new Vector3(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Vector3 operator /(Vector3 v1, float scalar)
        {
            if (scalar == 0)
                throw new DivideByZeroException("Cannot divide by zero");

            return new Vector3(v1.X / scalar, v1.Y / scalar, v1.Z / scalar);
        }

        public static Vector3 operator *(Vector3 v, double scalar)
        {
            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector3 operator *(double scalar, Vector3 v)
        {
            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        // ==, !=, other?
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            if (ReferenceEquals(v1, null) && ReferenceEquals(v2, null))
                return true;

            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
                return false;

            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        // If we overload == and !=, we should also override Equals and GetHashCode
        public override bool Equals(object obj)
        {
            Vector3 other = obj as Vector3;  //returns null if fail
            if (other == null)
                return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}
