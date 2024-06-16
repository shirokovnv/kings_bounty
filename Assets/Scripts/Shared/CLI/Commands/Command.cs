using System;

namespace Assets.Scripts.Shared.CLI.Commands
{
    public class Command : CommandBase
    {
        private readonly Action command;

        public Command(string id, string description, string format, Action command) : base(id, description, format)
        {
            this.command = command;
        }

        public override void Invoke(string[] args)
        {
            if (args.Length != 0)
            {
                return;
            }

            command.Invoke();
        }
    }

    public class Command<T> : CommandBase
    {
        private readonly Action<T> command;

        public Command(string id, string description, string format, Action<T> command) : base(id, description, format)
        {
            this.command = command;
        }

        public override void Invoke(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            command.Invoke(TryParse<T>(args[0]));
        }
    }

    public class Command<T1, T2> : CommandBase
    {
        private readonly Action<T1, T2> command;

        public Command(string id, string description, string format, Action<T1, T2> command) : base(id, description, format)
        {
            this.command = command;
        }

        public override void Invoke(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            command.Invoke(TryParse<T1>(args[0]), TryParse<T2>(args[1]));
        }
    }

}