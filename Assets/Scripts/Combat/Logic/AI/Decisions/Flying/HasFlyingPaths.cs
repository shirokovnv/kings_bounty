using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Decisions.Melee;
using Assets.Scripts.Combat.Logic.AI.Grid;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Flying
{
    public class HasFlyingPaths : DecisionNode
    {
        DecisionContext data;
        List<List<PathNode>> paths;

        public HasFlyingPaths(DecisionContext data)
        {
            this.data = data;
            paths = new List<List<PathNode>>();
        }

        public bool CheckCondition()
        {
            paths = AIPathFinder.FindFlyingPathsToEnemies(data);

            return paths.Count > 0;
        }

        public override Queue<CombatAction> Traverse()
        {
            if (CheckCondition())
            {
                var path = TargetSelector.ChooseRandomPath(paths);
                var lastNode = path.Last();

                UnitGroup oUnit = data.OGroups.Find(group => group.X == lastNode.X && group.Y == lastNode.Y);

                Next(new AirAttack(data.PGroup, oUnit, path));

                return next.Traverse();
            }

            Next(new PassTheTurn(data));

            return next.Traverse();
        }
    }
}