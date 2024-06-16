using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Shared.Data
{
    public class NamingData
    {
        private const string MAIN_CASTLE_NAME = "of King Maximus";
        private const string MAIN_TOWN_NAME = "Hunterville";

        private readonly static List<string> townNames = new()
    {
        "Arselgard",
        "Bayside",
        "Concotions",
        "Deepwater",
        "Everlasting",
        "Frostgard",
        "Grapesland",
        MAIN_TOWN_NAME,
        "Isla Vista",
        "Jars",
        "King's Haven",
        "Lakeview",
        "Midland",
        "Nyre",
        "Orangeland",
        "Pearls",
        "Quiln Point",
        "Riverton",
        "Silverton",
        "Thronehaim",
        "Underfoot",
        "Vanahaim",
        "Woodland",
        "Xoctan",
        "Yorvik",
        "Zeppelins"
    };

        private readonly static List<string> castleNames = new()
    {
        "Azram",
        "Barbaris",
        "Cancomar",
        "Duvock",
        "Evenmore",
        "Faxis",
        "Geladon",
        "Hammerfall",
        "Irok",
        "Jhan",
        MAIN_CASTLE_NAME,
        "Lorsche",
        "Moonstone",
        "Nilslag",
        "Ophiraund",
        "Portalis",
        "Quinderwitch",
        "Rythacon",
        "Stormfort",
        "Tylitch",
        "Uzare",
        "Vutar",
        "Wankelforte",
        "Xelox",
        "Yeneverre",
        "Zyxzax"
    };

        private readonly static List<string> signTitles = new()
    {
        "All maps are found in chests!",
        "Find the wood of elvish secrets.",
        "Secret pass.",
        "Treasures can be found in The Pirate's Cove.",
        "It takes time to cross the desert.",
        "History is written by the victors.",
        "Beware the wolves!",
        "Dead End",
        "Boon docks",
        "There are a couple of artifacts per continent.",
        "The Grail can be found with the help of bounty hunting.",
        "Rent a boat and sail the seas!",
        "The stars contain: you shall be victorious today!",
        "Lonesome? Spiritless? Prehaps wrestling an earth elemental will inject some vim into your routine."
    };

        private static NamingData instance;

        private Dictionary<string, bool> usedTownNames;
        private Dictionary<string, bool> usedCastleNames;

        private NamingData() { }

        public static NamingData Instance()
        {
            if (instance == null)
            {
                instance = new NamingData
                {
                    usedCastleNames = new Dictionary<string, bool>(),
                    usedTownNames = new Dictionary<string, bool>()
                };

                instance.ResetUsedTownNames();
                instance.ResetUsedCastleNames();
            }

            return instance;
        }

        public string GetUnusedCastleName()
        {
            string castleName = usedCastleNames
                .Where(kvp => kvp.Value == false)
                .OrderBy(_ => Random.value)
                .Select(kvp => kvp.Key)
                .FirstOrDefault();

            Debug.Log("Cas name:" + castleName);

            usedCastleNames[castleName] = true;

            return castleName;
        }

        public string GetUnusedTownName()
        {
            string townName = usedTownNames
                .Where(kvp => kvp.Value == false)
                .OrderBy(_ => Random.value)
                .Select(kvp => kvp.Key)
                .FirstOrDefault();

            usedTownNames[townName] = true;

            return townName;
        }

        public string GetMainCastleName()
        {
            usedCastleNames[MAIN_CASTLE_NAME] = true;

            return MAIN_CASTLE_NAME;
        }

        public string GetMainTownName()
        {
            usedTownNames[MAIN_TOWN_NAME] = true;

            return MAIN_TOWN_NAME;
        }

        public string GetRandomSignTitle()
        {
            return signTitles.ElementAt(Random.Range(0, signTitles.Count));
        }

        public NamingData ResetUsedTownNames()
        {
            usedTownNames.Clear();

            townNames.ForEach(name => usedTownNames.Add(name, false));

            return this;
        }

        public NamingData ResetUsedCastleNames()
        {
            usedCastleNames.Clear();

            castleNames.ForEach(name => usedCastleNames.Add(name, false));

            return this;
        }
    }
}