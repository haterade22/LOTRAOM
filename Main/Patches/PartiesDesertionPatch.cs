using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace LOTRAOM.Patches
{
    public class PartiesDesertionPatch
    {
        //[HarmonyPatch(typeof(DesertionCampaignBehavior), "PartiesCheckDesertionDueToPartySizeExceedsPaymentRatio")]
        public static bool Prefix(MobileParty mobileParty, ref TroopRoster desertedTroopList)
        {
            int partySizeLimit = mobileParty.Party.PartySizeLimit;
            if ((mobileParty.IsLordParty || mobileParty.IsCaravan) && mobileParty.Party.NumberOfAllMembers > partySizeLimit && mobileParty != MobileParty.MainParty && mobileParty.MapEvent == null)
            {
                int num = mobileParty.Party.NumberOfAllMembers - partySizeLimit;
                for (int i = 0; i < num; i++)
                {
                    CharacterObject character = mobileParty.MapFaction.BasicTroop;
                    int num2 = 99;
                    bool flag = false;
                    for (int j = 0; j < mobileParty.MemberRoster.Count; j++)
                    {
                        CharacterObject characterAtIndex = mobileParty.MemberRoster.GetCharacterAtIndex(j);
                        if (!characterAtIndex.IsHero && characterAtIndex.Level < num2 && mobileParty.MemberRoster.GetElementNumber(j) > 0)
                        {
                            num2 = characterAtIndex.Level;
                            character = characterAtIndex;
                            flag = mobileParty.MemberRoster.GetElementWoundedNumber(j) > 0;
                        }
                    }
                    if (num2 < 99)
                    {
                        if (flag)
                        {
                            mobileParty.MemberRoster.AddToCounts(character, -1, insertAtFront: false, -1);
                        }
                        else
                        {
                            mobileParty.MemberRoster.AddToCounts(character, -1);
                        }
                    }
                }
            }
            bool flag2 = mobileParty.IsWageLimitExceeded();
            if (!(mobileParty.Party.NumberOfAllMembers > mobileParty.LimitedPartySize || flag2))
            {
                return false;
            }
            int numberOfDeserters = Campaign.Current.Models.PartyDesertionModel.GetNumberOfDeserters(mobileParty);
            int num4;
            for (int k = 0; k < numberOfDeserters; k += num4)
            {
                if (mobileParty.MemberRoster.TotalRegulars <= 0)
                {
                    break;
                }
                int stackNo = -1;
                num4 = 1;
                for (int l = 0; l < mobileParty.MemberRoster.Count; l++)
                {
                    if (mobileParty.MemberRoster.TotalRegulars <= 0)
                    {
                        break;
                    }
                    CharacterObject characterAtIndex2 = mobileParty.MemberRoster.GetCharacterAtIndex(l);
                    int elementNumber = mobileParty.MemberRoster.GetElementNumber(l);
                    if (!characterAtIndex2.IsHero && elementNumber > 0)
                    {
                        stackNo = l;
                        num4 = Math.Min(elementNumber, numberOfDeserters - k);
                    }
                }
                MobilePartyHelper.DesertTroopsFromParty(mobileParty, stackNo, num4, 0, ref desertedTroopList);
            }
            return false;
        }
    }
}
