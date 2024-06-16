using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Systems;
using Assets.Scripts.Shared.CLI.Commands;
using Assets.Scripts.Shared.CLI.Handler;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared.CLI.Controller
{
    public class CommandController : MonoBehaviour
    {
        private bool showConsole;
        private bool showHelp;
        private Vector2 scroll;
        private string input;

        private CommandCollection commands;
        private ICommandHandler handler;

        private const int GUI_HEIGHT = 200;
        private const int INPUT_HEIGHT = 40;

        private static Command HELP;

        private static Command<int> ADD_GOLD;
        private static Command<int> ADD_LEADERSHIP;
        private static Command<int> ADD_COMMISSION;
        private static Command<int> ADD_SPELLPOWER;
        private static Command<int> ADD_KNOWLEDGE;
        private static Command<string, int> ADD_UNIT;
        private static Command<string, int> ADD_SPELL;

        private static Command BOON;

        private static Command REVEAL_ALL;
        private static Command<int, int> MOVE;
        private static Command<string> TRAVEL;

        private void Awake()
        {
            HELP = new Command("help", "displays all available commands", "help", () =>
            {
                showHelp = true;
            });

            ADD_GOLD = new Command<int>("add_gold", "adds an amount of gold", "add_gold <amount>", (amount) =>
            {
                PlayerStats.Instance().ChangeGold(amount);
            });

            ADD_LEADERSHIP = new Command<int>("add_leadership", "adds an amount of leadership", "add_leadership <amount>", (amount) =>
            {
                PlayerStats.Instance().ChangeLeadership(amount);
            });

            ADD_COMMISSION = new Command<int>("add_commission", "adds an amount of gold to your week commission", "add_commission <amount>", (amount) =>
            {
                PlayerStats.Instance().ChangeWeekSalary(amount);
            });

            ADD_SPELLPOWER = new Command<int>("add_spellpower", "adds an amount to your spellpower", "add_spellpower <amount>", (amount) =>
            {
                PlayerStats.Instance().ChangeSpellPower(amount);
            });

            ADD_KNOWLEDGE = new Command<int>("add_knowledge", "adds an amount to your knowledge", "add_knowledge <amount>", (amount) =>
            {
                PlayerStats.Instance().ChangeKnowledge(amount);
            });

            ADD_UNIT = new Command<string, int>("add_unit", "adds a unit to your armies", "add_unit <name> <quantity>", (name, quantity) =>
            {
                var unit = UnitManager.Instance().GetUnitByName(name);

                if (unit != null)
                {
                    PlayerSquads.Instance().AddOrRefillSquad(unit, quantity);
                }
            });

            ADD_SPELL = new Command<string, int>("add_spell", "adds a spell to your spellbook", "add_spell <name> <amount>", (name, amount) =>
            {
                var spell = SpellManager.Instance().GetSpellByName(name);

                if (spell != null)
                {
                    Spellbook.Instance().AddSpell(spell);
                }
            });

            BOON = new Command("boon", "increases total stats and adds some armies", "boon", () =>
            {
                ADD_GOLD.Invoke(new[] { "100000" });
                ADD_LEADERSHIP.Invoke(new[] { "10000" });
                ADD_COMMISSION.Invoke(new[] { "5000" });
                ADD_SPELLPOWER.Invoke(new[] { "10" });
                ADD_KNOWLEDGE.Invoke(new[] { "50" });
                ADD_UNIT.Invoke(new[] { "Dragons", "10" });

                REVEAL_ALL.Invoke(System.Array.Empty<string>());

                var spells = SpellManager.Instance().GetSpellScripts();
                foreach (var spell in spells)
                {
                    ADD_SPELL.Invoke(new[] { spell.Name, "5" });
                }
            });

            REVEAL_ALL = new Command("reveal_all", "Reveal all continent maps", "reveal_all", () =>
            {
                ContinentSystem.Instance().GetContinents().ForEach(c =>
                {
                    c.SetRevealed(true);
                    c.SetHasFullMap(true);
                });
            });

            MOVE = new Command<int, int>("move", "Moves to position", "move <X> <Y>", (x, y) =>
            {
                EventBus.Instance.PostEvent(new OnChangePosition(x, y));
            });

            TRAVEL = new Command<string>("travel", "Travels to continent", "travel <continent>", (name) =>
            {
                var continent = ContinentSystem.Instance().GetContinentAtName(name);

                if (continent != null)
                {
                    EventBus.Instance.PostEvent(new OnChangeContinent(continent.GetConfig().GetNumber()));
                }
            });

            commands = new CommandCollection();

            commands.AddCommand(HELP);

            commands.AddCommand(ADD_GOLD);
            commands.AddCommand(ADD_LEADERSHIP);
            commands.AddCommand(ADD_COMMISSION);
            commands.AddCommand(ADD_SPELLPOWER);
            commands.AddCommand(ADD_KNOWLEDGE);
            commands.AddCommand(ADD_UNIT);
            commands.AddCommand(ADD_SPELL);

            commands.AddCommand(BOON);

            commands.AddCommand(REVEAL_ALL);
            commands.AddCommand(MOVE);
            commands.AddCommand(TRAVEL);

            handler = new CommandHandler(commands);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ToggleConsole();
            }
        }

        private void ToggleConsole()
        {
            showHelp = false;
            showConsole = !showConsole;
        }

        private void OnGUI()
        {
            if (!showConsole) return;

            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return)
            {
                handler.Handle(input);
                input = string.Empty;
            }

            var y = 0f;

            GUIStyle style = new()
            {
                fontSize = 32
            };
            style.normal.textColor = Color.white;

            if (showHelp)
            {
                var commandList = commands.GetCommands().OrderBy(command => command.Id).ToList();
                GUI.Box(new Rect(0, y, Screen.width, GUI_HEIGHT), "");
                var viewPort = new Rect(0, 0, Screen.width - 30, INPUT_HEIGHT * commandList.Count);
                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, GUI_HEIGHT - 10), scroll, viewPort);
                for (var i = 0; i < commandList.Count; i++)
                {
                    var command = commandList[i];
                    var label = $"{command.Format} - {command.Description}";
                    var labelRect = new Rect(5, INPUT_HEIGHT * i, Screen.width - GUI_HEIGHT, INPUT_HEIGHT);
                    GUI.Label(labelRect, label, style);
                }
                GUI.EndScrollView();
                y += GUI_HEIGHT;
            }

            GUI.Box(new Rect(0, y, Screen.width, INPUT_HEIGHT), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - INPUT_HEIGHT, INPUT_HEIGHT), input, style);
        }
    }
}