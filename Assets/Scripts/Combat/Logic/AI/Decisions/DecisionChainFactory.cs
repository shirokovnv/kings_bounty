namespace Assets.Scripts.Combat.Logic.AI.Decisions
{
    public static class DecisionChainFactory
    {
        public static DecisionChain CreateByUnitType(DecisionContext context)
        {
            if (context.PGroup.IsPetrified)
            {
                return new PetrificationChain(context.PGroup);
            }

            if (context.PGroup.IsRanged())
            {
                return new RangedChain(context);
            }

            if (context.PGroup.IsFlyer())
            {
                return new FlyingChain(context);
            }

            return new MeleeChain(context);
        }
    }
}