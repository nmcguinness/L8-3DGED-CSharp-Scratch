namespace L8_3DGED_CSharp_Scratch
{

    /// <summary>
    /// A simple class representing a 3D vector with X, Y, and Z coordinates. Provides a demo of shallow and deep copy methods, as well as input validation for the Z coordinate.
    /// </summary>
    public class Vector3
    {
        private double x, y, z;

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

    }
}
