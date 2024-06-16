namespace Assets.Scripts.Combat.Logic.AI.Actions.Base
{
    public interface IAttackAction
    {
        public UnitGroup Attacker();
        public UnitGroup Defender();
    }
}