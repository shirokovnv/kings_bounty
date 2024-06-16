using Assets.Scripts.Adventure.Logic.Bounties;
using Assets.Scripts.Adventure.Logic.Continents;
using Assets.Scripts.Shared.Data.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Systems
{
    [System.Serializable]
    public class ContractSystem : ISerializationCallbackReceiver
    {
        private static ContractSystem instance;

        [SerializeField] private List<Contract> contracts;
        [SerializeField] private int currentContractIndex;

        private ContractSystem()
        {
        }

        public static ContractSystem Instance()
        {
            instance ??= new ContractSystem();

            return instance;
        }

        public List<Contract> GetContracts()
        {
            return contracts;
        }

        public void BuildContracts(List<Continent> continents, ContractBuilder contractBuilder)
        {
            var contractScripts = ContractManager.Instance().GetContracts();

            contracts = contractBuilder.Build(continents, contractScripts);
            currentContractIndex = -1;
        }

        public Contract GetNewContract()
        {
            var availableContractsCount = contracts.Where(contract => contract.IsFinished() == false).Count();

            if (availableContractsCount == 0)
            {
                currentContractIndex = -1;
                return null;
            }

            currentContractIndex = (currentContractIndex + 1) % contracts.Count;

            var contract = contracts.ElementAt(currentContractIndex);

            if (contract.IsFinished() == false)
            {
                return contract;
            }

            return GetNewContract();
        }

        public Contract GetCurrentContract()
        {
            if (currentContractIndex == -1)
            {
                return null;
            }

            return contracts[currentContractIndex];
        }

        public Contract GetContractByID(int id)
        {
            return contracts.Where(contract => contract.ContractInfo().GetScriptID() == id).FirstOrDefault();
        }

        public Contract GetContractByCastlePosition(int x, int y, int continentNumber)
        {
            return contracts.Where(contract =>
            {
                return contract.ContinentNumber() == continentNumber &&
                       contract.Position() == new Vector2Int(x, y);
            }).FirstOrDefault();
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            instance = this;
        }
    }
}