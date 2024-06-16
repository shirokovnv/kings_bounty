using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Shared.Data.Managers;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Bounties
{
    [System.Serializable]
    public class Contract : ISerializationCallbackReceiver
    {
        private const string UNKNOWN_TOKEN = "Unknown";

        private ContractScriptableObject contractInfo;
        [SerializeField] private int contractID;
        [SerializeField] private int x, y, continentNumber;
        [SerializeField] private int reward;
        [SerializeField] private bool isFinished;
        [SerializeField] private string castle;
        [SerializeField] private string lastSeen;

        public Contract(
            ContractScriptableObject contractInfo,
            int x,
            int y,
            int continentNumber,
            int reward
            )
        {
            this.contractInfo = contractInfo;
            this.x = x;
            this.y = y;
            this.continentNumber = continentNumber;
            this.reward = reward;
            isFinished = false;
            lastSeen = castle = UNKNOWN_TOKEN;
        }

        public int Reward()
        {
            return reward;
        }

        public int ContinentNumber()
        {
            return continentNumber;
        }

        public Vector2Int Position()
        {
            return new Vector2Int(x, y);
        }

        public bool IsFinished() { return isFinished; }
        public void SetFinished(bool isFinished) { this.isFinished = isFinished; }

        public ContractScriptableObject ContractInfo()
        {
            return contractInfo;
        }

        public string Castle()
        {
            return castle;
        }

        public string LastSeen()
        {
            return lastSeen;
        }

        public void SetLastSeen(string lastSeen) { this.lastSeen = lastSeen; }
        public void SetCastle(string castle) { this.castle = castle; }

        public void OnBeforeSerialize()
        {
            contractID = contractInfo.GetScriptID();
        }

        public void OnAfterDeserialize()
        {
            contractInfo = ContractManager.Instance().GetContractByID(contractID);
        }
    }
}