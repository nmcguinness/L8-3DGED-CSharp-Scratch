
using System;
using System.Collections.Generic;

namespace L8_3DGED_CSharp_Scratch.Demos
{
    public class Turret : IAttackPlayer
    {
        private Vector3 position;
        private IAttackStrategy attackStrategy;

        public Turret(Vector3 position, IAttackStrategy attackStrategy)
        {
            this.position = position;
            this.attackStrategy = attackStrategy;
        }

        public void Attack(List<Player> pList)
        {
            //Filter list through strategy 
            List<Player> targetList = attackStrategy.FilterBy(pList);

            foreach (Player p in targetList) {
                Console.WriteLine($"Attacking {p.ActorType} at {p.Position}");
            }
        }

    }
}
