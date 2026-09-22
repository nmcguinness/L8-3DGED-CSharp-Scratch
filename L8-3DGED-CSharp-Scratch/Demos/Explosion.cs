using System.Collections.Generic;

namespace L8_3DGED_CSharp_Scratch
{
    /// <summary>
    /// Applies damage to a set of targets at once.
    /// </summary>
    public class Explosion
    {
        private readonly List<IDamageable> _targets = new List<IDamageable>();

        public List<IDamageable> Targets => _targets;

        /// <summary>
        /// Registers a target to be damaged when this explosion detonates.
        /// </summary>
        /// <param name="target">The target to register.</param>
        public void Add(IDamageable target)
        {
            _targets.Add(target);
        }

        /// <summary>
        /// Damages every registered target.
        /// </summary>
        /// <param name="damage">Points of damage to apply to each target.</param>
        public void Detonate(int damage)
        {
            foreach (IDamageable target in _targets)
            {
                target.TakeDamage(damage);
            }
        }
    }
}
