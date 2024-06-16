using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Combat.Logic.Systems
{
    public enum MoraleGroup
    {
        A,
        B,
        C,
        D,
        E
    }

    public enum MoraleType
    {
        Low,
        Normal,
        High,
        OutOfControl,
    }

    public class MoraleSystem
    {
        private static MoraleSystem instance;

        private readonly static MoraleType[,] moraleTable =
        {
        { MoraleType.Normal, MoraleType.Normal, MoraleType.Normal, MoraleType.Low, MoraleType.Low },
        { MoraleType.Normal, MoraleType.Normal, MoraleType.Normal, MoraleType.Normal, MoraleType.Low },
        { MoraleType.Normal, MoraleType.Normal, MoraleType.High, MoraleType.Low, MoraleType.Low },
        { MoraleType.Normal, MoraleType.Normal, MoraleType.Normal, MoraleType.High, MoraleType.Normal },
        { MoraleType.Normal, MoraleType.Normal, MoraleType.Normal, MoraleType.Normal, MoraleType.Normal },
    };

        private MoraleSystem() { }

        public static MoraleSystem Instance()
        {
            instance ??= new MoraleSystem();

            return instance;
        }

        public void CalculateMorale(List<UnitGroup> squads)
        {
            List<MoraleType> morales = new();

            for (int i = 0; i < squads.Count; i++)
            {
                if (squads[i].IsOutOfControl())
                {
                    squads[i].Morale = MoraleType.OutOfControl;
                }
                else
                {
                    morales.Clear();

                    for (int j = 0; j < squads.Count; j++)
                    {
                        var morale = moraleTable[(int)squads[i].Unit.MoraleGroup, (int)squads[j].Unit.MoraleGroup];
                        morales.Add(morale);
                    }

                    squads[i].Morale = morales.Min();
                }
            }
        }
    }
}

