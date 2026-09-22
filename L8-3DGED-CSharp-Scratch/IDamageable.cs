namespace L8_3DGED_CSharp_Scratch
{
    /// <summary>
    /// A target that can receive damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Applies damage to this target.
        /// </summary>
        /// <param name="amount">Points of damage to apply.</param>
        void TakeDamage(int amount);
    }

    public class Barrel : IDamageable
    {
        private int _integrity = 20;

        /// <inheritdoc />
        public void TakeDamage(int amount)
        {
            _integrity -= amount;

            if (_integrity <= 0)
            {
                Explode();
            }
        }

        private void Explode()
        {
            // Spawn effect, remove from the scene.
        }
    }

    public class Enemy : IDamageable
    {
        private int _health = 100;
        private bool _isAlerted;

        /// <inheritdoc />
        public void TakeDamage(int amount)
        {
            _health -= amount;
            _isAlerted = true;
        }
    }
}
