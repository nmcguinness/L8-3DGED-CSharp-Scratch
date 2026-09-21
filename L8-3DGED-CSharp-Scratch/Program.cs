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


            Player p1 = new Player("Mage", 55, new Vector3(3, 2, 1));

            Player p2 = p1;  //point to the same object in memory

            Console.WriteLine(p2.Equals(p1)); //true

            var p3 = new Player("thief", 44, new Vector3(5, 6, 7));

            Console.WriteLine(p3.Equals(p1)); //false


            //use our new operator toys
            Vector3 v4 = 10 * v1 + v2 * 6;

            Console.WriteLine($"v4: {v4}"); 

        }
    }
}
