using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Combat.Interfaces;
using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Combat.Logic.Systems
{
    public class CombatSystem
    {
        private static CombatSystem instance;
        private BattleField battleField;

        public static CombatSystem Instance()
        {
            instance ??= new CombatSystem();

            return instance;
        }

        public BattleField GetBattleField()
        {
            return battleField;
        }

        public BattleField PrepareBattleField(ICombatable combatable, int width, int height)
        {
            battleField = null;

            var pList = PlayerSquads.Instance().GetSquads();
            var oList = combatable.GetSquads();

            switch (combatable.GetObject().GetObjectType())
            {
                case ObjectType.castleGate:
                    battleField = new CastleBattleField(width, height, pList, oList);
                    break;

                case ObjectType.captain:
                    battleField = new AdventureBattleField(width, height, pList, oList);
                    break;
            }

            return battleField;
        }
    }
}