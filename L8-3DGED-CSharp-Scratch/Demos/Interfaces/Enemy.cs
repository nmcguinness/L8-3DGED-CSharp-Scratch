namespace L8_3DGED_CSharp_Scratch
{
    public class Enemy : IDamageable
    {
        private static int _minAggressionLevel = 1, _maxAggressionLevel = 10;

        private int _health = 100;
        private int _aggressionLevel = 0;
        private bool _isAlerted;

        public Enemy(int health, int aggressionLevel, bool isAlerted)
        {
            _health = health;
            _aggressionLevel = aggressionLevel;
            _isAlerted = isAlerted;
        }

        public int Health { get => _health; protected set => _health = value; }
        public int AggressionLevel { get => _aggressionLevel; protected set => _aggressionLevel = value; }
        public bool IsAlerted { get => _isAlerted; protected set => _isAlerted = value; }
        public static int MinAggressionLevel { get => _minAggressionLevel; set => _minAggressionLevel = value; }
        public static int MaxAggressionLevel { get => _maxAggressionLevel; set => _maxAggressionLevel = value; }
        public static Enemy Barbarian
        {
            get
            {
                return new Enemy(100, _minAggressionLevel, false);
            }
        }
        public static Enemy Golem
        {
            get
            {
                return new Enemy(300, _maxAggressionLevel, true);
            }
        }

        /// <inheritdoc />
        public void TakeDamage(int amount)
        {
            _health -= amount;
            _isAlerted = true;
        }

        public override string ToString()
        {
            return $"Enemy(Health: {_health}, Aggression Level: {_aggressionLevel}, Alerted: {_isAlerted})";
        }
    }
}
