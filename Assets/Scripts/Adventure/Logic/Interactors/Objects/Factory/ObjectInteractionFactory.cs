using Assets.Scripts.Adventure.Logic.Continents.Base;

namespace Assets.Scripts.Adventure.Logic.Interactors.Objects.Factory
{
    public static class ObjectInteractionFactory
    {
        public static BaseObjectInteractor CreateInteractor(BaseObject worldObject)
        {
            return worldObject.GetObjectType() switch
            {
                ObjectType.chest => new ChestInteractor(),
                ObjectType.city => new TownInteractor(),
                ObjectType.castleGate => new CastleInteractor(),
                ObjectType.captain => new CaptainInteractor(),
                ObjectType.sign => new SignInteractor(),
                ObjectType.dwelling => new DwellingInteractor(),

                ObjectType.castleWall => new EmptyInteractor(),
                ObjectType.artifact => new ArtifactInteractor(),

                _ => throw new System.Exception("Unknown interactor."),
            };
        }
    }
}