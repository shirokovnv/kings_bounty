using Assets.Scripts.Adventure.Logic.Continents.Object;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared.Logic.Character
{
    [System.Serializable]
    public class Inventory
    {
        private static Inventory instance;
        [SerializeField] private List<Artifact> artifacts;

        public static Inventory Instance()
        {
            instance ??= new Inventory
            {
                artifacts = new List<Artifact>()
            };

            return instance;
        }

        public void AddArtifact(Artifact artifact)
        {
            artifacts.Add(artifact);
        }

        public List<Artifact> GetArtifacts()
        {
            return artifacts;
        }
    }
}