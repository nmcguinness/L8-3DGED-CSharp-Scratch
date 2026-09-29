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
}
