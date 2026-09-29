namespace L8_3DGED_CSharp_Scratch
{
    public class Barrel : IDamageable
    {
        private int _integrity = 20;

        public Barrel(int integrity)
        {
            _integrity = integrity;
        }



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

        public override string ToString()
        {
            return $"Barrel(Integrity: {_integrity})";
        }
    }
}
