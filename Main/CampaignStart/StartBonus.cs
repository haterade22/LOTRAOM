using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace LOTRAOM.CampaignStart
{
    public enum StartType
    {
        Default,
        Merchant,
        Exiled,
        Mercenary,
        Looter,
        VassalNoFief,
        KingdomRuler,
        CastleRuler,
        VassalFief,
        EscapedPrisoner
    }
    public class StartBonusDictionary
    {
        public StartBonusDictionary(int startGold, int startRenown, int numberOfCompanions, int numberOfCastles, int numberOfCities)
        {
            StartGold = startGold;
            StartRenown = startRenown;
            NumberOfCompanions = numberOfCompanions;
            NumberOfCastles = numberOfCastles;
            NumberOfCities = numberOfCities;
        }

        public int StartGold { get; set; }
        public int StartRenown { get; set; }
        public int NumberOfCompanions { get; set; }
        public int NumberOfCastles { get; set; }
        public int NumberOfCities { get; set; }
    }
    public class StartBonus
    {
        public static Dictionary<StartType, StartBonusDictionary> PlayerStartBonus = new()
        {
            [StartType.Default] = new(1000, 0, 0, 0, 0)
        };

        static StartBonus? _instance;
        public StartType StartTypeChosen { get; set; }
        public static StartBonus Instance
        {
            get
            {
                if (_instance == null) _instance = new StartBonus(); ;
                return _instance;
            }
        }
    }
}
