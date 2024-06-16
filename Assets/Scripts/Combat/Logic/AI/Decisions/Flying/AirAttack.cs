using Assets.Scripts.Combat.Logic.AI.Actions;
using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;

namespace Assets.Scripts.Combat.Logic.AI.Decisions.Flying
{
    public class AirAttack : DecisionNode
    {
        UnitGroup pUnit, oUnit;
        List<PathNode> path;

        public AirAttack(UnitGroup pUnit, UnitGroup oUnit, List<PathNode> path)
        {
            this.pUnit = pUnit;
            this.oUnit = oUnit;
            this.path = path;
        }

        public override Queue<CombatAction> Traverse()
        {
            var queue = new Queue<CombatAction>();
            queue.Enqueue(new FlyToPosition(pUnit, path));
            if (path.Count > 0)
            {
                queue.Enqueue(new Preparation());
            }
            queue.Enqueue(new Attack(pUnit, oUnit));

            if (!oUnit.IsDead() && oUnit.CurrentCounterstrikes > 0)
            {
                queue.Enqueue(new CounterAttack(oUnit, pUnit));
            }

            return queue;
        }
    }
}