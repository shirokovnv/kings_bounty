using Assets.Scripts.Adventure.Events;
using Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems;
using Assets.Scripts.Adventure.Logic.Continents.Object.Biome;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Events;
using Assets.Scripts.Shared.Logic.Character;
using Assets.Scripts.Shared.Data.State.Adventure;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors
{
    public class WorldInteractor : IInteractor<Adventuring>
    {
        private readonly int pX;
        private readonly int pY;
        private readonly Continent continent;
        private readonly bool isNavigating;

        public WorldInteractor(int pX, int pY, Continent continent, bool isNavigating)
        {
            this.pX = pX;
            this.pY = pY;
            this.continent = continent;
            this.isNavigating = isNavigating;
        }

        public void Interact(Adventuring state)
        {
            int dX = -1, dY = -1;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                dX = pX - 1;
                dY = pY;

                Player.Instance().SetDirection(Player.Direction.left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                dX = pX + 1;
                dY = pY;

                Player.Instance().SetDirection(Player.Direction.right);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                dX = pX;
                dY = pY + 1;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                dX = pX;
                dY = pY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                dX = pX - 1;
                dY = pY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                dX = pX + 1;
                dY = pY - 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                dX = pX - 1;
                dY = pY + 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                dX = pX + 1;
                dY = pY + 1;
            }

            var continentGrid = continent.GetGrid();

            // check edges
            if (dX < 0 || dY < 0 || dX >= continentGrid.GetWidth() || dY >= continentGrid.GetHeight())
            {
                return;
            }

            List<BiomeType> allowedBiomes = isNavigating
                ? new() { BiomeType.water }
                : new() { BiomeType.grass, BiomeType.desert };

            if (!isNavigating &&
                Player.Instance().GetWalkMode() == Player.WalkMode.Levitate)
            {
                allowedBiomes.AddRange(new[] { BiomeType.mountain, BiomeType.forest });
            }

            var worldTile = continentGrid.GetValue(dX, dY);

            List<BiomeType> groundBiomes = new() { BiomeType.grass, BiomeType.desert };

            // check shore
            if (
                isNavigating &&
                groundBiomes.Contains(worldTile.BiomeLayer.GetBiomeType()) &&
                worldTile.HasObject() == false
                )
            {
                object[] events = { new OnShore(dX, dY), new OnChangePosition(dX, dY) };
                EventBus.Instance.PostEventGroup(events);

                return;
            }

            // check boat
            if (!isNavigating && dX == Boat.Instance().X && dY == Boat.Instance().Y)
            {
                object[] events = { new OnBoat(dX, dY), new OnChangePosition(dX, dY) };
                EventBus.Instance.PostEventGroup(events);

                return;
            }

            // check objects
            if (worldTile.HasObject())
            {
                GameStateManager.Instance().SetState(new InteractingWithObject
                {
                    Stage = InteractingWithObject.InteractingStage.entering,
                    Obj = worldTile.ObjectLayer
                });

                return;
            }

            // check world
            if (allowedBiomes.Contains(worldTile.BiomeLayer.GetBiomeType()))
            {
                // spend amount of time
                if (worldTile.BiomeLayer.GetBiomeType() == BiomeType.desert)
                {
                    TimeSystem.Instance().Tick(
                        TimeSystem.Instance().GetMovesPerDay() / Player.Instance().GetEndurance()
                        );
                }
                else
                {
                    TimeSystem.Instance().Tick();
                }

                EventBus.Instance.PostEvent(new OnChangePosition(dX, dY));

                CaptainMovementSystem.Instance().MoveCaptains(continent, dX, dY);

                return;
            }
        }
    }
}