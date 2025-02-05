﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace LOTRAOM.CampaignStart
{
    static class CampaignStartGlobals
    {
        public static Dictionary<string, string> StartingSettlement = new()
        {
            // if the mod keeps has multiple factions with same culture (like empire in vanilla) this code will have to be refactored
             ["aserai"] = "town_EW1",
             ["empire"] = "town_EW1",
             ["khuzait"] = "town_EW1",
             ["sturgia"] = "town_EW1",
             ["battania"] = "town_EW1",
             ["vlandia"] = "town_EW1",
             ["gondor"] = "town_EW1",
             ["mordor"] = "town_ES1",
             ["rivendell"] = "town_EW1",
             ["mirkwood"] = "town_EW1",
             ["lothlorien"] = "town_EW1",
             ["erebor"] = "town_EW1",
             ["umbar"] = "town_EW1",
             ["dolguldur"] = "town_EW1",
             ["isengard"] = "town_EW1",
             ["gundabad"] = "town_EW1",
        };
        public static void OnCharacterCreationFinalized()
        {
        }
    }
}
