using Assets.Resources.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shared.Data.Managers
{
    public class ArtifactManager
    {
        private static ArtifactManager instance;
        private List<ArtifactScriptableObject> artifactScripts;

        public static ArtifactManager Instance()
        {
            if (instance == null)
            {
                instance = new ArtifactManager();
                instance.artifactScripts = UnityEngine.Resources.LoadAll<ArtifactScriptableObject>("ScriptableObjects").ToList();
            }

            return instance;
        }

        public List<ArtifactScriptableObject> GetArtifacts() { return artifactScripts; }

        public List<ArtifactScriptableObject> GetArtifactsByLevel(int level)
        {
            return artifactScripts.Where(artifact => artifact.Level == level).ToList();
        }

        public ArtifactScriptableObject GetArtifactByID(int id)
        {
            return artifactScripts.Where(artifact => artifact.GetScriptID() == id).FirstOrDefault();
        }
    }
}