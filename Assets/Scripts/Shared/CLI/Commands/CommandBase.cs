using System.ComponentModel;
using System.Globalization;

namespace Assets.Scripts.Shared.CLI.Commands
{
    abstract public class CommandBase
    {
        public string Id { get; protected set; }
        public string Description { get; protected set; }
        public string Format { get; protected set; }

        protected CommandBase(string id, string description, string format)
        {
            Id = id;
            Description = description;
            Format = format;
        }

        abstract public void Invoke(string[] args);

        protected static T TryParse<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
        }
    }
}