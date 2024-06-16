using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;

namespace Assets.Scripts.Shared.Logic.Bonuses.Temporary
{
    [System.Serializable]
    public class StandStillBonus : TemporaryBonus
    {
        public StandStillBonus(int duration, bool isStackable) : base(duration, isStackable)
        {
        }

        public override string Message()
        {
            return $"STILL MODE : {duration}";
        }

        protected override void Commit()
        {
            CaptainMovementSystem.Instance().SetMode(CaptainMovementSystem.CaptainMode.Stand);
        }

        protected override void Rollback()
        {
            CaptainMovementSystem.Instance().SetMode(CaptainMovementSystem.CaptainMode.Move);
        }
    }
}