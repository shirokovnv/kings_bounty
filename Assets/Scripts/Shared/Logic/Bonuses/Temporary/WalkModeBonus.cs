using Assets.Scripts.Shared.Logic.Character;

namespace Assets.Scripts.Shared.Logic.Bonuses.Temporary
{
    public class WalkModeBonus : TemporaryBonus
    {
        public WalkModeBonus(int duration, bool isStackable) : base(duration, isStackable)
        {
        }

        public override BonusEnum GetBonusEnumValue()
        {
            return BonusEnum.WalkModeBonus;
        }

        public override string Message()
        {
            return $"LEVITATION : {duration}";
        }

        protected override void Commit()
        {
            Player.Instance().SetWalkMode(Player.WalkMode.Levitate);
        }

        protected override void Rollback()
        {
            Player.Instance().SetWalkMode(Player.WalkMode.Normal);
        }
    }
}