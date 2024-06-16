namespace Assets.Scripts.Combat.Logic.AI.Actions.Base
{
    abstract public class CombatDamageAction : CombatAction
    {
        protected const float CRITICAL_DAMAGE_CHANCE = 0.3f;
        protected const int MIN_CRIT_MULTIPLIER = 2;
        protected const int MAX_CRIT_MULTIPLIER = 5;

        protected int damageDealt;
        protected bool isCritical;
        protected int critMultiplier;
    }
}