using Assets.Scripts.Adventure.Logic.Bounties;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Puzzle
{
    public class PuzzleBag : IPuzzleBag
    {
        private readonly List<Continent> continents;
        private readonly List<Contract> contracts;

        public PuzzleBag(List<Continent> continents, List<Contract> contracts)
        {
            this.continents = continents;
            this.contracts = contracts;
        }

        public List<PuzzlePiece> GetPuzzles()
        {
            List<PuzzlePiece> puzzles = new();

            var artifactPuzzles = continents
                .SelectMany(c => c.GetObjectsOfType(ObjectType.artifact))
                .ToList()
                .Select(artifact => new PuzzlePiece
                {
                    Type = PuzzlePiece.PuzzleType.Artifact,
                    ID = (artifact as Artifact).ArtifactScript.GetScriptID(),
                    Revealed = false,
                }).ToList();

            var contractPuzzles = contracts
                .Select(contract => new PuzzlePiece
                {
                    Type = PuzzlePiece.PuzzleType.Contract,
                    ID = contract.ContractInfo().GetScriptID(),
                    Revealed = false,
                }).ToList();

            return puzzles.Concat(artifactPuzzles.Concat(contractPuzzles)).ToList();
        }
    }
}