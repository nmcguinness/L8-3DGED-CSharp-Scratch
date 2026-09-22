namespace L8_3DGED_CSharp_Scratch
{
    public class Enemy : IDamageable
    {
        private int _health = 100;
        private bool _isAlerted;

        public Enemy(int health, bool isAlerted)
        {
            _health = health;
            _isAlerted = isAlerted;
        }

        /// <inheritdoc />
        public void TakeDamage(int amount)
        {
            _health -= amount;
            _isAlerted = true;
        }

        public override string ToString()
        {
            return $"Enemy(Health: {_health}, Alerted: {_isAlerted})";
        }
    }
}
