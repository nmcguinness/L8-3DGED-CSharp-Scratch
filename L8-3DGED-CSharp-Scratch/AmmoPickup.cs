using System;

namespace L8_3DGED_CSharp_Scratch
{
    internal class AmmoPickup : PickupBase
    {
        public AmmoPickup(int amount) : base(amount)
        {
    
        }

        protected override void OnCollected()  //concrete impl. of abstract
        {
            Console.WriteLine("Making ammo pickup sound!");
        }
    }
}