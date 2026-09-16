using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L8_3DGED_CSharp_Scratch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var v1 = new Vector3(1, 2, 3);

            var v2 = v1.ShallowCopy(); //makes a shallow copy of v1

            v1.X = 100; //change some values in v1

            Console.WriteLine($"v1: {v1} and v2: {v2}"); //compare v1 and v2 to see if they are the same or different

            var v3 = v1.DeepCopy(); //makes a deep copy of v1

            v1.Y  = 200; //change some values in v1

            Console.WriteLine($"v1: {v1} and v3: {v3}"); //compare v1 and v3 to see if they are the same or different

        }
    }
}
