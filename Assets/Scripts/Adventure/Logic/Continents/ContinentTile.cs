using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents
{
    [System.Serializable]
    public class ContinentTile : ISerializationCallbackReceiver
    {
        [SerializeField] private bool isRevealed;
        [SerializeField] private BiomeObject biomeLayer;
        [SerializeField] private ObjectPackage objectPackage;
        private BaseObject objectLayer;

        public ContinentTile(BiomeObject biomeLayer = null, BaseObject objectLayer = null, bool isRevealed = false)
        {
            this.biomeLayer = biomeLayer;
            this.objectLayer = objectLayer;
            this.isRevealed = isRevealed;
        }

        public BiomeObject BiomeLayer { get { return biomeLayer; } set { biomeLayer = value; } }
        public BaseObject ObjectLayer { get { return objectLayer; } set { objectLayer = value; } }

        public bool IsRevealed() { return isRevealed; }

        public void SetRevealed(bool isRevealed)
        {
            this.isRevealed = isRevealed;
        }

        public bool IsEmptyGround(BiomeType type = BiomeType.grass)
        {
            return biomeLayer.GetBiomeType() == type && objectLayer == null;
        }

        public bool HasObject()
        {
            return objectLayer != null;
        }

        public bool HasObject(ObjectType type)
        {
            return objectLayer?.GetObjectType() == type;
        }

        public void SetBiome(BiomeType type = BiomeType.grass)
        {
            biomeLayer.SetBiomeType(type);
        }

        public void OnBeforeSerialize()
        {
            objectPackage = new ObjectPackage(objectLayer);
        }

        public void OnAfterDeserialize()
        {
            objectLayer = objectPackage.GetObject();
        }
    }
}