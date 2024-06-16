using Assets.Scripts.Adventure.Logic.Continents.Builder;

namespace Assets.Scripts.Adventure.Logic.Continents.Config
{
    public sealed class ConfigPackage
    {
        public string Name;
        public ContinentConfig Config;
        public IContinentBuilder Builder;
        public IContinentDirector Director;
    }
}