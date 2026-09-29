using System;

namespace L8_3DGED_CSharp_Scratch.Demos
{

    /// <summary>
    /// Represents a drawn UI healthbar in Unity/Unreal/Godot
    /// </summary>
    public class HealthBar
    {
        private int _currentHealth;
        private int _maxHealth;

        public HealthBar(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void Refresh(int delta, PickupType pickupType)
        {
            //Version A - extra time
            //_currentHealth = 
            //    (_currentHealth + delta <= _maxHealth)
            //    ? _currentHealth + delta //if its small enough to add, then add
            //    : _maxHealth;   //if its too big, then max at upper limit

            //Version B - extra 4 bytes (int)
            //int newHealth = _currentHealth + delta;
            //_currentHealth = newHealth <= _maxHealth ? newHealth : _maxHealth;

      
            //Version C
            _currentHealth += 
                (_currentHealth + delta <= _maxHealth)
                ? delta 
                : _maxHealth;

            Console.WriteLine($"Setting new difficulty to {_currentHealth}");

        }
    }
}
