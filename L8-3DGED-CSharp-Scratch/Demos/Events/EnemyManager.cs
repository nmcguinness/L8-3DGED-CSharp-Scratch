using System;

namespace L8_3DGED_CSharp_Scratch.Demos
{
    public class EnemyManager
    {
        private Enemy _enemyArchetype;

        public EnemyManager(Enemy enemyArchetype)
        {
            _enemyArchetype = enemyArchetype;
        }

        /*TODO*/

        public void HandleDifficultyChanged(int newDiff)
        {
            Console.WriteLine($"Reacting to new difficulty {newDiff} by modifying enemy aggression level");
        }
    }
}
