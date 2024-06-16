using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Logic.Character.Spells.Targeting;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Adventure.UI.Dialog;
using Assets.Resources.ScriptableObjects;

namespace Assets.Scripts.Adventure.Logic.Interactors.Dialogs
{
    public class CastSpellInteractor : BaseDialogInteractor<CastSpell, DidCastSpell>
    {
        private static readonly Dictionary<string, System.Action<SpellScriptableObject>> portals = new()
    {
        { "Town gate", PortalToTown },
        { "Castle gate", PortalToCastle },
    };

        private static readonly KeyCode[] keyCodes = new KeyCode[]
        {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
        KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
        KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
        KeyCode.Z,
        };

        private const int NUM_ELEMENTS_PER_ROW = 5;

        public override void WaitForStart()
        {
            var state = GameStateManager.Instance().GetState() as CastSpell;

            if (portals.Keys.Contains(state.Spell.Name))
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DialogUI.Instance.HideDialog();

                    GameStateManager.Instance().SetState(new Adventuring());

                    return;
                }

                portals[state.Spell.Name](state.Spell);

                return;
            }

            Spellbook.Instance().UseSpell(state.Spell);

            GameStateManager.Instance().SetState(new DidCastSpell { Spell = state.Spell });
        }

        public override void WaitForFinish()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DialogUI.Instance.HideDialog();

                GameStateManager.Instance().SetState(new Adventuring());
            }
        }

        private static void PortalToTown(SpellScriptableObject spell)
        {
            var visitedTowns = ContinentSystem
                    .Instance()
                    .GetContinents()
                    .SelectMany(continent => continent.GetCities().Where(town => town.IsVisited()))
                    .ToList();

            if (visitedTowns.Count() == 0)
            {
                DialogUI.Instance.UpdateTextMessage("You have no visited cities yet.");

                GameStateManager.Instance().SetState(new DidCastSpell { Spell = spell });

                return;
            }

            string text = string.Empty;
            for (int i = 0; i < visitedTowns.Count; i++)
            {
                text += char.ToUpper(System.Convert.ToChar(keyCodes[i])) + ") " + visitedTowns[i].GetName();

                if (i != visitedTowns.Count - 1)
                {
                    text += ", ";
                }

                if ((i + 1) % NUM_ELEMENTS_PER_ROW == 0)
                {
                    text += "\r\n";
                }

                DialogUI.Instance.UpdateTextMessage(text);

                if (Input.GetKeyDown(keyCodes[i]))
                {
                    var target = visitedTowns[i];

                    var continent = ContinentSystem.Instance().GetContinentAtNumber(target.ContinentNumber);

                    var packages = continent.GetTileRectWithPositions(target.X, target.Y, 1, 1);

                    var biomeType = Player.Instance().IsNavigating()
                        ? BiomeType.water
                        : BiomeType.grass;

                    PortalTarget portalTarget = portalTarget = packages
                            .Where(p => p.Tile.IsEmptyGround(biomeType))
                            .Select(p => new PortalTarget { X = p.X, Y = p.Y, ContinentNumber = target.ContinentNumber })
                            .OrderBy(p => Random.value)
                            .FirstOrDefault();

                    Spellbook.Instance().UseSpell(spell, portalTarget);

                    GameStateManager.Instance().SetState(new DidCastSpell { Spell = spell });
                }
            }
        }

        private static void PortalToCastle(SpellScriptableObject spell)
        {
            var visitedCastles = ContinentSystem
                    .Instance()
                    .GetContinents()
                    .SelectMany(continent => continent.GetCastles().Where(castle => castle.IsVisited()))
                    .ToList();

            if (visitedCastles.Count() == 0)
            {
                DialogUI.Instance.UpdateTextMessage("You have no visited castles yet.");

                GameStateManager.Instance().SetState(new DidCastSpell { Spell = spell });

                return;
            }

            string text = string.Empty;
            for (int i = 0; i < visitedCastles.Count; i++)
            {
                text += char.ToUpper(System.Convert.ToChar(keyCodes[i])) + ") " + visitedCastles[i].GetName();

                if (i != visitedCastles.Count - 1)
                {
                    text += ", ";
                }

                if ((i + 1) % NUM_ELEMENTS_PER_ROW == 0)
                {
                    text += "\r\n";
                }

                DialogUI.Instance.UpdateTextMessage(text);

                if (Input.GetKeyDown(keyCodes[i]))
                {
                    var target = visitedCastles[i];

                    PortalTarget portalTarget = new()
                    {
                        X = target.X,
                        Y = target.Y - 1,
                        ContinentNumber = target.ContinentNumber,
                    };

                    Spellbook.Instance().UseSpell(spell, portalTarget);

                    GameStateManager.Instance().SetState(new DidCastSpell { Spell = spell });
                }
            }
        }
    }
}