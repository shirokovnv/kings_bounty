using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class MoveToPosition : CombatAction
    {
        private UnitGroup group;
        private List<PathNode> path;
        private readonly bool targetOpponent;
        private string message;

        public MoveToPosition(UnitGroup group, List<PathNode> path, bool targetOpponent = true)
        {
            this.group = group;
            this.path = path;
            this.targetOpponent = targetOpponent;

            if (path.Count < 2)
            {
                throw new System.Exception("Incorrect path.");
            }

            path.RemoveAt(0);
            if (targetOpponent)
            {
                path.RemoveAt(path.Count - 1);
            }
        }

        public override void Execute()
        {
            if (!IsCompleted())
            {
                var nextNode = path.ElementAt(0);
                message = $"Move {group.Message()} to {nextNode.X} {nextNode.Y}";

                group.X = nextNode.X;
                group.Y = nextNode.Y;
                group.CurrentMoves--;

                path.RemoveAt(0);
            }
        }

        public override bool IsCompleted()
        {
            return group.CurrentMoves <= 0 || path.Count == 0;
        }

        public override string Message()
        {
            return message;
        }
    }
}