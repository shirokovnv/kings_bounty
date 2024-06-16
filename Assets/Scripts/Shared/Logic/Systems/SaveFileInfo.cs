using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;
using Assets.Scripts.Shared.Logic.Character;
using System;

namespace Assets.Scripts.Shared.Logic.Systems
{
    [Serializable]
    public class SaveFileInfo
    {
        public Player Player = Player.Instance();
        public Boat Boat = Boat.Instance();
        public Inventory Inventory = Inventory.Instance();
        public PlayerStats PlayerStats = PlayerStats.Instance();
        public PlayerSquads PlayerSquads = PlayerSquads.Instance();
        public Spellbook Spellbook = Spellbook.Instance();

        public ContinentSystem ContinentSystem = ContinentSystem.Instance();
        public ContractSystem ContractSystem = ContractSystem.Instance();
        public PuzzleSystem PuzzleSystem = PuzzleSystem.Instance();
        public TimeSystem TimeSystem = TimeSystem.Instance();
        public CaptainMovementSystem CaptainMovementSystem = CaptainMovementSystem.Instance();
    }
}