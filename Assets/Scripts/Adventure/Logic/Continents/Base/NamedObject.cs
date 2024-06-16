using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Base
{
    [System.Serializable]
    abstract public class NamedObject
    {
        [SerializeField] protected string name;

        protected NamedObject(string name)
        {
            this.name = name;
        }

        public string GetName() { return name; }
        public void SetName(string name) { this.name = name; }
    }
}