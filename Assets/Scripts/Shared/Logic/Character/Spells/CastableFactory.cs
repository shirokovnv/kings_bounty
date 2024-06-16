using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Shared.Logic.Character.Spells.Adventure;
using Assets.Scripts.Shared.Logic.Character.Spells.Combat;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;

namespace Assets.Scripts.Shared.Logic.Character.Spells
{
    public static class CastableFactory
    {
        public static ICastable Create(SpellScriptableObject spell, SpellTarget target = null)
        {
            return spell.Name switch
            {
                // Travel spells
                "Charisma" => new CharismaSpell(),
                "Summoning" => new SummoningSpell(),
                "Castle gate" => new CastleGateSpell(target as PortalTarget),
                "Town gate" => new TownGateSpell(target as PortalTarget),
                "Stand still" => new StandStillSpell(),
                "Reveal contract" => new RevealContractSpell(),
                "Levitation" => new LevitationSpell(),

                // Combat spells
                "Fireball" => new FireballSpell(target as CombatSpellTarget),
                "Lightning bolt" => new LightningBoltSpell(target as CombatSpellTarget),
                "Petrify" => new PetrifySpell(target as CombatSpellTarget),
                "Resurrect" => new ResurrectSpell(target as CombatSpellTarget),
                "Teleport" => new TeleportSpell(target as CombatSpellTarget),
                "Clone" => new CloneSpell(target as CombatSpellTarget),
                "Exorcism" => new ExorcismSpell(target as CombatSpellTarget),

                _ => null,
            };
        }
    }
}