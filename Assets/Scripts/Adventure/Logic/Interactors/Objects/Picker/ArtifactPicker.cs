using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Shared.Logic.Bonuses.Modifiers;
using Assets.Scripts.Shared.Logic.Character;
using System.Collections.Generic;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects.Picker
{
    public static class ArtifactPicker
    {
        private static readonly Dictionary<string, System.Action> pickupActions = new()
    {
        { "Amulet of Augmentation", OnAmuletOfAugmentationPickUp },
        { "Anchor of Admiralty", OnAnchorOfAdmiraltyPickUp },
        { "Articles of Nobility", OnArticlesOfNobilityPickUp },
        { "Crown of Command", OnCrownOfCommandPickUp },
        { "Ring of Snake", OnRingOfHeroismPickUp },
        { "Shield of Protection", OnShieldOfProtectionPickUp },
        { "Sword of Prowess", OnSwordOfProwessPickUp },
        { "Tome of Knowledge", OnTomeOfKnowledgePickUp },
    };

        public static void PickUp(ArtifactScriptableObject artifact)
        {
            if (pickupActions.ContainsKey(artifact.Name))
            {
                pickupActions[artifact.Name].Invoke();
            }
        }

        private static void OnAmuletOfAugmentationPickUp()
        {
            PlayerStats.Instance().ChangeSpellPower(5);
        }

        private static void OnAnchorOfAdmiraltyPickUp()
        {
            Boat.Instance().Discount = 400;
        }

        private static void OnArticlesOfNobilityPickUp()
        {
            PlayerStats.Instance().ChangeWeekSalary(2000);
        }

        private static void OnCrownOfCommandPickUp()
        {
            PlayerStats.Instance().ChangeLeadership(
                PlayerStats.Instance().GetLeadership()
                );
        }

        private static void OnRingOfHeroismPickUp()
        {
            Player.Instance().SetEndurance(2);
        }

        private static void OnShieldOfProtectionPickUp()
        {
            PlayerSquads
                .Instance()
                .AddModifier(new DefenceModifier(0.25f, false));
        }

        private static void OnSwordOfProwessPickUp()
        {
            PlayerSquads
                .Instance()
                .AddModifier(new AttackModifier(0.5f, false));
        }

        private static void OnTomeOfKnowledgePickUp()
        {
            PlayerStats.Instance().ChangeKnowledge(
                PlayerStats.Instance().GetKnowledge()
                );
        }
    }
}