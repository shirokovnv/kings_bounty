using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Utility;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems
{
    [System.Serializable]
    public class CaptainMovementSystem : ISerializationCallbackReceiver
    {
        private const int SEARCH_RADIUS = 3;

        public enum CaptainMode
        {
            Stand,
            Move
        }

        [SerializeField] private CaptainMode mode;

        private static CaptainMovementSystem instance;

        private CaptainMovementSystem() { }

        public static CaptainMovementSystem Instance()
        {
            instance ??= new CaptainMovementSystem();

            return instance;
        }

        public CaptainMode GetMode() { return mode; }
        public void SetMode(CaptainMode mode) { this.mode = mode; }

        public void MoveCaptains(
            Continent continent,
            int pX,
            int pY
            )
        {
            if (mode == CaptainMode.Stand)
            {
                return;
            }

            var searchArea = continent.GetTileRectWithPositions(pX, pY, SEARCH_RADIUS, SEARCH_RADIUS);
            var captains = searchArea
                .Where(package => package.Tile.HasObject(ObjectType.captain))
                .Select(package => package.Tile.ObjectLayer as Captain)
                .Where(captain => Utils.ManDistance(pX, pY, captain.X, captain.Y) > 1)
                .ToList();

            if (captains.Count == 0)
            {
                return;
            }

            captains.ForEach(captain =>
            {
                var siblings = continent.GetTileRectWithPositions(captain.X, captain.Y, 1, 1);
                var positions = siblings
                    .Where(sibling =>
                    {
                        return sibling.Tile.IsEmptyGround() &&
                            continent.GetGrid().GetValue(sibling.X, sibling.Y + 1).HasObject(ObjectType.castleGate) == false &&
                            Utils.ManDistance(pX, pY, sibling.X, sibling.Y) < Utils.ManDistance(pX, pY, captain.X, captain.Y);
                    })
                    .Select(sibling => new Vector2Int(sibling.X, sibling.Y))
                    .ToList();

                if (positions.Count > 0)
                {
                    var position = positions.ElementAt(0);
                    var moveCaptainEvent = new OnMoveCaptain
                    {
                        X = captain.X,
                        Y = captain.Y,
                        TargetX = position.x,
                        TargetY = position.y,
                        ContinentNumber = continent.GetConfig().GetNumber()
                    };

                    continent.GetGrid().GetValue(captain.X, captain.Y).ObjectLayer = null;

                    captain.X = position.x;
                    captain.Y = position.y;

                    continent.GetGrid().GetValue(captain.X, captain.Y).ObjectLayer = captain;

                    EventBus.Instance.PostEvent(moveCaptainEvent);
                }
            });
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