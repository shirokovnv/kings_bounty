using Assets.Scripts.Combat.Logic.AI.Actions.Base;
using Assets.Scripts.Shared.Grid;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Combat.Logic.AI.Actions
{
    public class FlyToPosition : CombatAction
    {
        private UnitGroup pUnit;
        private List<PathNode> path;
        private bool isCompleted;
        private string message;

        public FlyToPosition(UnitGroup pUnit, List<PathNode> path)
        {
            this.pUnit = pUnit;
            this.path = path;
            isCompleted = false;

            if (path.Count < 2)
            {
                throw new System.Exception("Incorrect flying path.");
            }

            path.RemoveAt(0);
            path.RemoveAt(path.Count - 1);
        }


        public override void Execute()
        {
            if (isCompleted)
            {
                return;
            }

            if (path.Count > 0)
            {
                var targetNode = path.ElementAt(0);
                message = $"Fly {pUnit.Message()} to {targetNode.X}, {targetNode.Y}";

                pUnit.X = targetNode.X;
                pUnit.Y = targetNode.Y;
                path.RemoveAt(0);
            }

            isCompleted = true;
        }

        public override bool IsCompleted()
        {
            return isCompleted;
        }

        public override string Message()
        {
            return message;
        }
    }
}