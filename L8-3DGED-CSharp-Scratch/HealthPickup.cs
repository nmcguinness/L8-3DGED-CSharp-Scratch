using System;

namespace L8_3DGED_CSharp_Scratch
{
    internal class HealthPickup : PickupBase
    {
        public HealthPickup(int amount) : base(amount)
        {

        }

        protected override void OnCollected()
        {
            Console.WriteLine("Making health pickup sound!");
        }
    }
}