using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Base
{
    [System.Serializable]
    abstract public class BaseObject : NamedObject
    {
        [SerializeField] protected ObjectType type;

        public int X, Y, ContinentNumber;

        public BaseObject(string name, ObjectType type) : base(name)
        {
            this.type = type;
        }

        public ObjectType GetObjectType() { return type; }
    }
}