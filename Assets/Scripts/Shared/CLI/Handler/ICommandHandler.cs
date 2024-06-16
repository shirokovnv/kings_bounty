namespace Assets.Scripts.Shared.CLI.Handler
{
    public interface ICommandHandler
    {
        public bool Handle(string input);
    }
}