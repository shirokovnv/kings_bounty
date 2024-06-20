using Assets.Resources.ScriptableObjects;
using Assets.Scripts.Adventure.Logic.Continents;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Bounties
{
    public class ContractBuilder
    {
        private const int GOLD_MULTIPLIER = 100;

        public List<Contract> Build(List<Continent> continents, List<ContractScriptableObject> contractScripts)
        {
            List<Castle> castles = new();

            continents.ForEach(continent => { castles.AddRange(continent.GetCastles()); });
            castles.RemoveAll((castle) => castle.GetOwner() == CastleOwner.king);

            List<Contract> contracts = new();

            foreach (var contractInfo in contractScripts)
            {
                var castleIndex = Random.Range(0, castles.Count);
                var castle = castles.ElementAt(castleIndex);

                var reward = castle.GetStrength() * GOLD_MULTIPLIER;

                var contract = new Contract(contractInfo, castle.X, castle.Y, castle.ContinentNumber, reward);
                contracts.Add(contract);
                castle.SetIsContracted(true);

                castles.RemoveAt(castleIndex);
            }

            contracts = contracts
                .OrderBy(contract => contract.ContinentNumber())
                .ThenBy(contract => contract.Reward())
                .ToList();

            foreach (var contract in contracts)
            {
                Debug.Log(
                    contract.ContractInfo().Title +
                    " (" + contract.ContinentNumber() + ") " +
                    contract.Position() + " : " +
                    contract.Reward());
            }

            return contracts;
        }
    }
}