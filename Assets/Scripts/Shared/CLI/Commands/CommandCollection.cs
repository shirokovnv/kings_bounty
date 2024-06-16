using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shared.CLI.Commands
{
    public class CommandCollection
    {
        private readonly Dictionary<string, CommandBase> commands;

        public CommandCollection()
        {
            commands = new Dictionary<string, CommandBase>();
        }

        public CommandBase GetCommand(string commandId)
        {
            if (commands.TryGetValue(commandId, out CommandBase command))
            {
                return command;
            }

            return null;
        }

        public void AddCommand(CommandBase command)
        {
            commands.Add(command.Id, command);
        }

        public List<CommandBase> GetCommands()
        {
            return commands.Values.ToList();
        }
    }
}