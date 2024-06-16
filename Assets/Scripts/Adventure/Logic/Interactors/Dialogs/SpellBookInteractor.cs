using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using System;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Dialog;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class SpellBookInteractor : BaseDialogInteractor<Adventuring, ViewSpellBook>
    {
        private readonly KeyCode[] keyCodes = new KeyCode[]
        {
                KeyCode.A,
                KeyCode.B,
                KeyCode.C,
                KeyCode.D,
                KeyCode.E,
                KeyCode.F,
                KeyCode.G,
        };

        public override void WaitForStart()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                var spellbook = Spellbook.Instance();

                string title = $"Choose spell";
                string text = string.Empty;

                int index = 0;
                foreach (var (spell, amount) in spellbook.GetTravelSpells())
                {
                    text += char.ToUpper(Convert.ToChar(keyCodes[index])) + ") "
                         + spell.Name + " : " + amount + "\r\n";
                    index++;
                }

                DialogUI.Instance.ShowDialogUI(title, text, null);

                GameStateManager.Instance().SetState(new ViewSpellBook());
            }
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameStateManager.Instance().SetState(new Adventuring());
                DialogUI.Instance.HideDialog();
            }

            for (int i = 0; i < keyCodes.Length; i++)
            {
                var spellCounter = Spellbook.Instance().GetTravelSpells().ElementAt(i);

                if (Input.GetKeyDown(keyCodes[i]) &&
                    spellCounter.Value > 0)
                {
                    GameStateManager.Instance().SetState(
                        new CastSpell { Spell = spellCounter.Key }
                        );
                }
            }
        }
    }
}