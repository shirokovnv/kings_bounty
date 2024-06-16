using Assets.Scripts.Shared.CLI.Commands;
using System.Linq;

namespace Assets.Scripts.Shared.CLI.Handler
{
    public class CommandHandler : ICommandHandler
    {
        private readonly CommandCollection commands;

        public CommandHandler(CommandCollection commands)
        {
            this.commands = commands;
        }

        public bool Handle(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var splitInput = input.Split(' ');
            var commandId = splitInput.First();
            var args = splitInput.Skip(1).ToArray();

            var command = commands.GetCommand(commandId);
            if (command == null)
            {
                return false;
            }

            command.Invoke(args);
            return true;
        }
    }
}