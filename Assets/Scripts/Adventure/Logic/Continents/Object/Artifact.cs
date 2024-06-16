using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Shared.Data.Managers;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Object
{
    [System.Serializable]
    public class Artifact : BaseObject, ISerializationCallbackReceiver
    {
        [SerializeField] private int ArtifactID;

        [System.NonSerialized] public ArtifactScriptableObject ArtifactScript;

        public Artifact(int x, int y, int continentNumber, ArtifactScriptableObject artifactScript)
        : base(artifactScript.Name, ObjectType.artifact)
        {
            X = x;
            Y = y;
            ContinentNumber = continentNumber;
            ArtifactScript = artifactScript;
        }

        public void OnAfterDeserialize()
        {
            ArtifactScript = ArtifactManager.Instance().GetArtifactByID(ArtifactID);
        }

        public void OnBeforeSerialize()
        {
            ArtifactID = ArtifactScript.GetScriptID();
        }
    }
}