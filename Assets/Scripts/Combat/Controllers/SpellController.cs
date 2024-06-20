using Assets.Scripts.Combat.Events;
using Assets.Scripts.Combat.UI;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Combat.Controllers
{
    public class SpellController : MonoBehaviour
    {
        private static readonly KeyCode[] keyCodes = new KeyCode[]
        {
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        };

        private enum State
        {
            None,
            ViewSpellbook,
            CastSpell,
        }

        private State state;
        private BattleField battleField;
        private CombatSpellTarget spellTarget;
        private int sourceX, sourceY;

        private void Awake()
        {
            state = State.None;

            battleField = null;
            spellTarget = null;
            sourceX = sourceY = -1;

            EventBus.Instance.Register(this);
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case State.None:
                    break;

                case State.ViewSpellbook:
                    ProcessViewSpellbookInput();
                    break;

                case State.CastSpell:
                    ProcessCastSpellInput();
                    break;
            }
        }

        private void ProcessViewSpellbookInput()
        {
            var spells = Spellbook.Instance().GetCombatSpells();

            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]) && spells.Values.ElementAt(i) > 0)
                {
                    // cast spell
                    state = State.CastSpell;
                    spellTarget = new CombatSpellTarget
                    {
                        Source = spells.Keys.ElementAt(i),
                        Target = null,
                        X = -1,
                        Y = -1,
                    };

                    CursorController.Instance.ActivateWithPosition(
                        sourceX,
                        sourceY,
                        battleField.Grid.GetWidth(),
                        battleField.Grid.GetHeight()
                        );
                    CombatDialogScript.Instance.HideDialog();
                    CombatConsoleScript.Instance.PushMessage(
                        $"Spell {spellTarget.Source.Name} activated."
                        );
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = State.None;
                CombatDialogScript.Instance.HideDialog();
                EventBus.Instance.PostEvent(new OnHideSpellbook());
                CombatConsoleScript.Instance.PushMessage(
                    $"Spell {spellTarget.Source.Name} deactivated."
                    );
            }
        }

        private void ProcessCastSpellInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                state = State.None;
                CombatDialogScript.Instance.HideDialog();
                CursorController.Instance.Deactivate();
                EventBus.Instance.PostEvent(new OnHideSpellbook());
                CombatConsoleScript.Instance.PushMessage(
                    $"Spell {spellTarget.Source.Name} deactivated."
                    );
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (IsSpellTargetReady(
                    CursorController.Instance.GetCursorX(),
                    CursorController.Instance.GetCursorY())
                    )
                {
                    EventBus.Instance.PostEvent(new OnChooseSpell { SpellTarget = spellTarget });
                    CursorController.Instance.Deactivate();
                    state = State.None;
                    spellTarget = null;
                    sourceX = sourceY = -1;

                    return;
                }

                if (spellTarget.Source.Name != "Teleport")
                {
                    CombatConsoleScript.Instance.PushMessage("Not a valid target");
                }
            }
        }

        private bool IsSpellTargetReady(int X, int Y)
        {
            if (spellTarget == null)
            {
                return false;
            }

            return spellTarget.Source.Name switch
            {
                "Fireball" => CheckDirectDamageTarget(X, Y),
                "Lightning bolt" => CheckDirectDamageTarget(X, Y),
                "Exorcism" => CheckDirectDamageTarget(X, Y, true),
                "Teleport" => CheckTeleportTarget(X, Y),
                "Petrify" => CheckNegativeEffectTarget(X, Y),
                "Clone" => CheckPositiveEffectTarget(X, Y),
                "Resurrect" => CheckPositiveEffectTarget(X, Y),
                _ => false
            };
        }

        private bool CheckDirectDamageTarget(int X, int Y, bool checkUndead = false)
        {
            bool isEnemyTarget = battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.opponent);
            bool isOutOfControlTarget =
                battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.player) &&
                battleField.PList.Where(p => p.X == X && p.Y == Y && p.IsOutOfControl()).Any();

            if (isEnemyTarget || isOutOfControlTarget)
            {
                var targetList = isEnemyTarget
                    ? battleField.OList
                    : battleField.PList;

                var target = targetList.Where(o => o.X == X && o.Y == Y).FirstOrDefault();

                if (target == null)
                {
                    return false;
                }

                if (checkUndead && !target.IsUndead())
                {
                    return false;
                }

                spellTarget.X = X;
                spellTarget.Y = Y;
                spellTarget.Target = target;

                return true;
            }

            return false;
        }

        private bool CheckTeleportTarget(int X, int Y)
        {
            if (spellTarget.Target == null)
            {
                UnitGroup target = null;

                if (battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.opponent))
                {
                    target = battleField.OList.Where(o => o.X == X && o.Y == Y).FirstOrDefault();
                }

                if (battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.player))
                {
                    target = battleField.PList.Where(p => p.X == X && p.Y == Y).FirstOrDefault();
                }

                spellTarget.Target = target;

                if (target == null)
                {
                    CombatConsoleScript.Instance.PushMessage("Not a valid teleport target");
                }

                return false;
            }

            if (spellTarget.X == -1 && spellTarget.Y == -1)
            {
                if (!battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.player) &&
                    !battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.opponent) &&
                    !battleField.Grid.GetValue(X, Y).IsObstacle())
                {
                    spellTarget.X = X;
                    spellTarget.Y = Y;

                    return true;
                }

                CombatConsoleScript.Instance.PushMessage("Not a valid place to teleport");
            }

            return false;
        }

        private bool CheckNegativeEffectTarget(int X, int Y)
        {
            bool isEnemyTarget = battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.opponent);
            bool isOutOfControlTarget =
                battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.player) &&
                battleField.PList.Where(p => p.X == X && p.Y == Y && p.IsOutOfControl()).Any();

            if (isEnemyTarget || isOutOfControlTarget)
            {
                var targetList = isEnemyTarget
                    ? battleField.OList
                    : battleField.PList;

                spellTarget.Target = targetList.Where(o => o.X == X && o.Y == Y).FirstOrDefault();

                return true;
            }

            return false;
        }

        private bool CheckPositiveEffectTarget(int X, int Y)
        {
            if (battleField.BelongsTo(X, Y, UnitGroup.UnitOwner.player))
            {
                spellTarget.Target = battleField.PList.Where(o => o.X == X && o.Y == Y).FirstOrDefault();

                if (spellTarget.Source.Name == "Resurrect" &&
                    spellTarget.Target.InitialQuantity < spellTarget.Target.CurrentQuantity())
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public void OnEvent(OnViewSpellbook e)
        {
            sourceX = e.SourceX;
            sourceY = e.SourceY;
            battleField = e.BattleField;
            ShowSpellbook();
        }

        private void ShowSpellbook()
        {
            state = State.ViewSpellbook;

            var spells = Spellbook.Instance().GetCombatSpells();

            string title = "Choose spell";
            string text = string.Empty;

            int index = 0;
            foreach (var (spell, count) in spells)
            {
                text += $"{char.ToUpper(System.Convert.ToChar(keyCodes[index]))}) {spell.Name} : {count} \r\n";
                index++;
            }

            CombatDialogScript.Instance.ShowDialog(title, text);
        }
    }
}