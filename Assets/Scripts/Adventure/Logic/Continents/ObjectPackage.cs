using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using System;
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
            ObjectLayerType = obj?.GetObjectType().ToString();
        }

        public readonly BaseObject GetObject()
        {
            if (ObjectLayerStr == null || ObjectLayerStr == string.Empty) { return null; }

            if (ObjectLayerType == null || ObjectLayerType == string.Empty) { return null; }

            if (!Enum.TryParse(ObjectLayerType, out ObjectType baseObjectType))
            {
                throw new Exception("Unknown object type.");
            }

            return baseObjectType switch
            {
                ObjectType.chest => JsonUtility.FromJson<Chest>(ObjectLayerStr),
                ObjectType.dwelling => JsonUtility.FromJson<Dwelling>(ObjectLayerStr),
                ObjectType.artifact => JsonUtility.FromJson<Artifact>(ObjectLayerStr),
                ObjectType.captain => JsonUtility.FromJson<Captain>(ObjectLayerStr),
                ObjectType.castleGate => JsonUtility.FromJson<Castle>(ObjectLayerStr),
                ObjectType.castleWall => JsonUtility.FromJson<CastleWall>(ObjectLayerStr),
                ObjectType.sign => JsonUtility.FromJson<Sign>(ObjectLayerStr),
                ObjectType.city => JsonUtility.FromJson<City>(ObjectLayerStr),
                _ => null,
            };
        }
    }
}