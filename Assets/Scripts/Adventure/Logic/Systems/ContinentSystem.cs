using Assets.Scripts.Adventure.Logic.Continents;
using Assets.Scripts.Adventure.Logic.Continents.Builder;
using Assets.Scripts.Adventure.Logic.Continents.Config;
using Assets.Scripts.Adventure.Logic.Continents.Object;
using Assets.Scripts.Adventure.Logic.Continents.Spec;
using Assets.Scripts.Shared.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Adventure.Logic.Continents.Interactors.Systems
{
    [System.Serializable]
    public class ContinentSystem
    {
        private const int MAX_ITERATIONS_COUNT = 1000;

        private static ContinentSystem instance;
        [SerializeField] private List<Continent> continents;

        public static ContinentSystem Instance()
        {
            instance ??= new ContinentSystem
            {
                continents = new List<Continent>()
            };

            return instance;
        }

        public void Generate(ContinentConfigPack configPack)
        {
            continents.Clear();

            List<bool> attempts = new();
            foreach (var config in configPack.Configs)
            {
                attempts.Add(
                    RecursiveGenerateContinent(
                        config.Name,
                        config.Config,
                        config.Builder,
                        config.Director,
                        configPack.Specification)
                );
            }

            var isSatisfied = attempts.Aggregate(true, (acc, attempt) => acc && attempt);

            if (!isSatisfied)
            {
                throw new System.Exception("Could not generate. Increase attempts count.");
            }

            continents = continents
                .OrderBy(c => c.GetConfig()
                .GetNumber())
                .ToList();

            var mainCastle = continents
                .ElementAt(0)
                .GetCastles()
                .Where(castle => castle.GetOwner() == CastleOwner.king)
                .FirstOrDefault();

            // set names
            continents.ForEach(continent =>
            {
                continent.GetCastles().ForEach(castle =>
                {
                    if (castle.GetOwner() == CastleOwner.king)
                    {
                        castle.SetName(NamingData.Instance().GetMainCastleName());
                    }

                    if (castle.GetOwner() == CastleOwner.opponent)
                    {
                        castle.SetName(NamingData.Instance().GetUnusedCastleName());
                    }
                });

                continent.GetCities().ForEach(city =>
                {
                    if (city.GetLinkedCastle() == mainCastle)
                    {
                        city.SetName(NamingData.Instance().GetMainTownName());
                    }
                    else
                    {
                        city.SetName(NamingData.Instance().GetUnusedTownName());
                    }
                });
            });
        }

        private bool RecursiveGenerateContinent(
            string name,
            ContinentConfig config,
            IContinentBuilder builder,
            IContinentDirector director,
            ISpecification<Continent> spec,
            int iteration = 0
            )
        {
            if (iteration >= MAX_ITERATIONS_COUNT)
            {
                return false;
            }

            var grid = director.Make(builder);
            var continent = new Continent(name, config, grid);

            if (spec.IsSatisfiedBy(continent))
            {
                continents.Add(continent);

                return true;
            }

            return RecursiveGenerateContinent(name, config, builder, director, spec, iteration + 1);
        }

        public List<Continent> GetContinents()
        {
            return continents;
        }

        public List<Continent> GetRevealedContinents()
        {
            return continents.Where(continent => continent.IsRevealed()).ToList();
        }

        public Continent GetContinentAtNumber(int number)
        {
            var index = number - 1;

            if (index < 0 || index >= continents.Count) return null;

            return continents.ElementAt(index);
        }

        public Continent GetContinentAtName(string name)
        {
            return continents.Where(c => c.GetName().Equals(name)).FirstOrDefault();
        }
    }
}