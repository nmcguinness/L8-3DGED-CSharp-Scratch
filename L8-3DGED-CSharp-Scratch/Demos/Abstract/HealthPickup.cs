using System;

namespace L8_3DGED_CSharp_Scratch
{
    public class HealthPickup : PickupBase
    {
        public HealthPickup(int amount) : base(amount)
        {

        }

        protected override void OnCollected()  //concrete impl. of abstract
        {
            Console.WriteLine("Making health pickup sound!");
        }
    }
}