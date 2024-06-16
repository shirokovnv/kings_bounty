using Assets.Scripts.Adventure.Logic.Continents.Base;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Shared.Data.Managers;
using Assets.Scripts.Shared.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Spec
{
    public class ContinentSpecification : ISpecification<Continent>
    {
        private const int MIN_WATER_SIBLINGS_COUNT = 3;

        public bool IsSatisfiedBy(Continent continent)
        {
            List<Func<Continent, bool>> specParts = new()
        {
            HasProperNumberOfCastlesAndTowns,
            HasProperNumberOfMapsInChests,
            HasMainCastleWhenFirstContinent,
            HasProperNumberOfDwellings,
            HasProperNumberOfArtifacts,
            HasProperNumberOfCaptains,
            HasWaterNearAllTowns,
            HasPathBetweenMainCastleAndTown
        };

            return specParts.Aggregate(true, (acc, part) => part(continent) && acc);
        }

        private bool HasProperNumberOfCastlesAndTowns(Continent continent)
        {
            int citiesCount = continent.GetConfig().GetObjectConfig().GetCitiesCount();
            int castlesCount = continent.GetConfig().GetObjectConfig().GetCastlesCount();

            if (continent.GetConfig().GetNumber() == 1)
            {
                citiesCount++;
                castlesCount++;
            }

            if (citiesCount != continent.GetCities().Count || castlesCount != continent.GetCastles().Count)
            {
                return false;
            }

            var result = continent.GetCities().Aggregate(true, (acc, city) => city.GetLinkedCastle() != null && acc);

            return result;
        }

        private bool HasProperNumberOfCaptains(Continent continent)
        {
            var captains = continent.GetObjectsOfType(ObjectType.captain);

            return
                captains.Count >= continent.GetConfig().GetObjectConfig().GetMinCaptainsCount() &&
                captains.Count <= continent.GetConfig().GetObjectConfig().GetMaxCaptainsCount();
        }

        private bool HasProperNumberOfMapsInChests(Continent continent)
        {
            int continentNumber = continent.GetConfig().GetNumber();
            var chests = continent.GetObjectsOfType(ObjectType.chest);

            if (chests.Where(chest => (chest as Chest).GetChestType() == Chest.ChestType.MapReveal).Count() != 1)
            {
                return false;
            }

            int countMapsToTheNextContinent = continentNumber < continent.GetConfig().GetMaxContinentsCount()
                ? 1
                : 0;

            if (chests.Where(chest => (chest as Chest).GetChestType() == Chest.ChestType.PathToContinent).Count() != countMapsToTheNextContinent)
            {
                return false;
            }

            return true;
        }

        public bool HasMainCastleWhenFirstContinent(Continent continent)
        {
            if (continent.GetConfig().GetNumber() != 1)
            {
                return true;
            }

            return continent.GetCastles().Where(castle => castle.GetOwner() == CastleOwner.king).Count() == 1;
        }

        public bool HasProperNumberOfDwellings(Continent continent)
        {
            int minDwellingsCount = continent.GetConfig().GetObjectConfig().GetMinDwellingsCount();
            int maxDwellingsCount = continent.GetConfig().GetObjectConfig().GetMaxDwellingsCount();

            var dwellingsCount = continent.GetObjectsOfType(ObjectType.dwelling).Count();

            return dwellingsCount >= minDwellingsCount && dwellingsCount <= maxDwellingsCount;
        }

        public bool HasProperNumberOfArtifacts(Continent continent)
        {
            var artifactsCount = continent.GetObjectsOfType(ObjectType.artifact).Count();

            var artifactsCountInManager = ArtifactManager.Instance().GetArtifactsByLevel(
                continent.GetConfig().GetNumber()
                ).Count();

            return artifactsCount == artifactsCountInManager;
        }

        public bool HasWaterNearAllTowns(Continent continent)
        {
            return continent.GetCities().Aggregate(true, (acc, city) =>
                city.GetWaterSiblings().Count >= MIN_WATER_SIBLINGS_COUNT && acc);
        }

        public bool HasPathBetweenMainCastleAndTown(Continent continent)
        {
            if (continent.GetConfig().GetNumber() != 1)
            {
                return true;
            }

            var mainCastle = continent.GetCastles()
                .Where(castle => castle.GetOwner() == CastleOwner.king)
                .FirstOrDefault();

            if (mainCastle == null)
            {
                return false;
            }

            var mainCity = continent.GetCities()
                .Where(city => city.GetLinkedCastle() == mainCastle)
                .FirstOrDefault();

            if (mainCity == null)
            {
                return false;
            }

            PathFinder pathFinder = new(continent.GetConfig().GetWidth(), continent.GetConfig().GetHeight());
            var pathGrid = pathFinder.GetGrid();
            var continentGrid = continent.GetGrid();

            for (int x = 0; x < pathGrid.GetWidth(); x++)
            {
                for (int y = 0; y < pathGrid.GetHeight(); y++)
                {
                    pathGrid.GetValue(x, y).IsWalkable = continentGrid.GetValue(x, y).IsEmptyGround();

                    if (continentGrid.GetValue(x, y).HasObject(ObjectType.chest))
                    {
                        pathGrid.GetValue(x, y).IsWalkable = true;
                    }
                }
            }

            pathGrid.GetValue(mainCastle.X, mainCastle.Y).IsWalkable = true;
            pathGrid.GetValue(mainCity.X, mainCity.Y).IsWalkable = true;

            var path = pathFinder.FindPath(
                new Vector2Int(mainCastle.X, mainCastle.Y),
                new Vector2Int(mainCity.X, mainCity.Y)
                );

            return path != null && path.Count > 0;
        }
    }
}