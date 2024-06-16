using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Combat.Logic.AI.Grid;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Melee
{
    public class PathsToOpponents : DecisionNode
    {
        DecisionContext data;
        List<List<PathNode>> paths;

        public PathsToOpponents(DecisionContext data)
        {
            this.data = data;
        }

        public bool CheckCondition()
        {
            paths = AIPathFinder.FindPathsToEnemies(data);

            return paths.Count > 0;
        }

        public override Queue<CombatAction> Traverse()
        {
            DecisionNode next = CheckCondition()
                ? new LookAround(data, paths)
                : new PassTheTurn(data);

            Next(next);

            return next.Traverse();
        }
    }
}