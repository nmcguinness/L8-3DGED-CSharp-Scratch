using System;

namespace L8_3DGED_CSharp_Scratch
{
    public abstract class PickupBase //cannot be instantiated
    {
        protected int amount;

        protected PickupBase(int amount)  // only child classes can call this
        {
            this.amount = amount;
        }
        public void Collect()
        {
            Console.WriteLine($"A pickup worth {amount} was just collected!");
            OnCollected();
        }

        protected abstract void OnCollected();
    }
}