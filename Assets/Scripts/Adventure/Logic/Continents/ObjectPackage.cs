using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents
{
    [System.Serializable]
    public struct ObjectPackage
    {
        public string ObjectLayerStr;
        public string ObjectLayerType;

        public ObjectPackage(BaseObject obj)
        {
            ObjectLayerStr = JsonUtility.ToJson(obj);
            ObjectLayerType = obj?.GetType().ToString();
        }

        public readonly BaseObject GetObject()
        {
            if (ObjectLayerStr == null) { return null; }

            return ObjectLayerType switch
            {
                nameof(Chest) => JsonUtility.FromJson<Chest>(ObjectLayerStr),
                nameof(Dwelling) => JsonUtility.FromJson<Dwelling>(ObjectLayerStr),
                nameof(Artifact) => JsonUtility.FromJson<Artifact>(ObjectLayerStr),
                nameof(Captain) => JsonUtility.FromJson<Captain>(ObjectLayerStr),
                nameof(Castle) => JsonUtility.FromJson<Castle>(ObjectLayerStr),
                nameof(CastleWall) => JsonUtility.FromJson<CastleWall>(ObjectLayerStr),
                nameof(Sign) => JsonUtility.FromJson<Sign>(ObjectLayerStr),
                nameof(City) => JsonUtility.FromJson<City>(ObjectLayerStr),
                _ => null,
            };
        }
    }
}