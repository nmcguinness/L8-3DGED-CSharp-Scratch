using System.Collections.Generic;

namespace L8_3DGED_CSharp_Scratch.Demos
{
    public interface IAttackStrategy
    {
        List<Player> FilterBy(List<Player> potentialTargets);
    }

    public class ActorProximityStrategy : IAttackStrategy
    {
        private string TargetActorType;
        private float distanceThreshold;

        public ActorProximityStrategy(string targetActorType, float distanceThreshold)
        {
            TargetActorType = targetActorType;
            this.distanceThreshold = distanceThreshold;
        }

        public List<Player> FilterBy(List<Player> potentialTargets)
        {
            List<Player> targetList = new List<Player>();

            foreach (Player target in potentialTargets)
            {
                if(target.ActorType.Equals(TargetActorType))
                {
                    if(target.Position.X < 50 && target.Position.X > 0)
                    {
                        targetList.Add(target);
                    }
                }
            }
            return targetList;
        }
    }
}