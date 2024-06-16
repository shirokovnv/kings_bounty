using Assets.Resources.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Shared.Data.Managers
{
    public class ContractManager
    {
        private static ContractManager instance;
        private List<ContractScriptableObject> contractScripts;

        public static ContractManager Instance()
        {
            instance ??= new ContractManager
            {
                contractScripts = UnityEngine.Resources.LoadAll<ContractScriptableObject>("ScriptableObjects").ToList()
            };

            return instance;
        }

        public List<ContractScriptableObject> GetContracts()
        {
            return contractScripts;
        }

        public ContractScriptableObject GetContractByID(int id)
        {
            return contractScripts.Where(contract => contract.GetScriptID() == id).FirstOrDefault();
        }
    }
}