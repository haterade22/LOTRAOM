//using LOTRAOM.BalanceOfPower.Actions;
//using LOTRAOM.BalanceOfPower.Pacts;
//using LOTRAOM.Diplomacy.Pacts.Diplomacy.DiplomaticAction;
//using LOTRAOM.Extensions;
//using System.Collections.Generic;
//using TaleWorlds.CampaignSystem;
//using TaleWorlds.Library;


//namespace LOTRAOM.BalanceOfPower
//{
//    internal sealed class CampaignCheatsExtension
//    {
//        [CommandLineFunctionality.CommandLineArgumentFunction("form_alliance", "diplomacy")]
//        private static string FormAlliance(List<string> strings)
//        {
//            if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
//            {
//                return CampaignCheats.ErrorType;
//            }
//            if (!CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
//            {
//                return "Format uses 2 kingdom ID parameters without spaces: diplomacy.form_alliance [Kingdom1] [Kingdom2]";
//            }
//            string text = strings[0].ToLower();
//            string text2 = strings[1].ToLower();
//            if (text == text2)
//            {
//                return "Cannot ally a kingdom to itself!";
//            }
//            Kingdom kingdom = null;
//            Kingdom kingdom2 = null;
//            foreach (Kingdom kingdom3 in KingdomExtensions.AllActiveKingdoms)
//            {
//                string a = kingdom3.Name.ToString().ToLower().Replace(" ", "");
//                if (a == text)
//                {
//                    kingdom = kingdom3;
//                }
//                else if (a == text2)
//                {
//                    kingdom2 = kingdom3;
//                }
//            }
//            if (kingdom == null && kingdom2 == null)
//            {
//                return "Could not find either required kingdom!";
//            }
//            if (kingdom == null)
//            {
//                return "1st kingdom ID not found: " + text;
//            }
//            if (kingdom2 == null)
//            {
//                return "2nd kingdom ID not found: " + text2;
//            }
//            AbstractDiplomaticPact<DeclareAllianceAction>.Apply(kingdom, kingdom2, false, true, null, true);
//            return string.Format("Alliance formed between {0} and {1}!", kingdom.Name, kingdom2.Name);
//        }

//        [CommandLineFunctionality.CommandLineArgumentFunction("form_non_aggression_pact", "diplomacy")]
//        private static string FormNonAggressionPact(List<string> strings)
//        {
//            if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
//            {
//                return CampaignCheats.ErrorType;
//            }
//            if (!CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
//            {
//                return "Format uses 2 kingdom ID parameters without spaces: diplomacy.form_non_aggression_pact [Kingdom1] [Kingdom2]";
//            }
//            string text = strings[0].ToLower();
//            string text2 = strings[1].ToLower();
//            if (text == text2)
//            {
//                return "Cannot make a pact with itself!";
//            }
//            Kingdom kingdom = null;
//            Kingdom kingdom2 = null;
//            foreach (Kingdom kingdom3 in KingdomExtensions.AllActiveKingdoms)
//            {
//                string a = kingdom3.Name.ToString().ToLower().Replace(" ", "");
//                if (a == text)
//                {
//                    kingdom = kingdom3;
//                }
//                else if (a == text2)
//                {
//                    kingdom2 = kingdom3;
//                }
//            }
//            if (kingdom == null && kingdom2 == null)
//            {
//                return "Could not find either required kingdom!";
//            }
//            if (kingdom == null)
//            {
//                return "1st kingdom ID not found: " + text;
//            }
//            if (kingdom2 == null)
//            {
//                return "2nd kingdom ID not found: " + text2;
//            }
//            NonAggressionAgreement nonAggressionPactAgreement;
//            if (!DiplomaticAgreementManager.HasNonAggressionPact(kingdom, kingdom2, out nonAggressionPactAgreement))
//            {
//                AbstractDiplomaticAction<FormNonAggressionPactAction>.Apply(kingdom, kingdom2, false, true, null, true);
//                return string.Format("Non-aggression pact formed between {0} and {1}!", kingdom.Name, kingdom2.Name);
//            }
//            return "Specified kingdoms already have a non-aggression pact!";
//        }

//        [CommandLineFunctionality.CommandLineArgumentFunction("break_non_aggression_pact", "diplomacy")]
//        private static string BreakNonAggressionPact(List<string> strings)
//        {
//            if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
//            {
//                return CampaignCheats.ErrorType;
//            }
//            if (!CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
//            {
//                return "Format uses 2 kingdom ID parameters without spaces: diplomacy.form_non_aggression_pact [Kingdom1] [Kingdom2]";
//            }
//            string text = strings[0].ToLower();
//            string text2 = strings[1].ToLower();
//            if (text == text2)
//            {
//                return "Cannot break a pact with itself!";
//            }
//            Kingdom kingdom = null;
//            Kingdom kingdom2 = null;
//            foreach (Kingdom kingdom3 in KingdomExtensions.AllActiveKingdoms)
//            {
//                string a = kingdom3.Name.ToString().ToLower().Replace(" ", "");
//                if (a == text)
//                {
//                    kingdom = kingdom3;
//                }
//                else if (a == text2)
//                {
//                    kingdom2 = kingdom3;
//                }
//            }
//            if (kingdom == null && kingdom2 == null)
//            {
//                return "Could not find either required kingdom!";
//            }
//            if (kingdom == null)
//            {
//                return "1st kingdom ID not found: " + text;
//            }
//            if (kingdom2 == null)
//            {
//                return "2nd kingdom ID not found: " + text2;
//            }
//            NonAggressionAgreement nonAggressionPactAgreement;
//            if (DiplomaticAgreementManager.HasNonAggressionPact(kingdom, kingdom2, out nonAggressionPactAgreement))
//            {
//                nonAggressionPactAgreement.Expire();
//                return string.Format("Non-aggression pact broken between {0} and {1}!", kingdom.Name, kingdom2.Name);
//            }
//            return "Specified kingdoms don't have a non-aggression pact!";
//        }

//        //[CommandLineFunctionality.CommandLineArgumentFunction("legitimize_rebel_kingdom", "diplomacy")]
//        //[UsedImplicitly]
//        //public static string LegitimizeRebelKingdom(List<string> strings)
//        //{
//        //    if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
//        //    {
//        //        return CampaignCheats.ErrorType;
//        //    }
//        //    if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
//        //    {
//        //        return "Format uses 1 kingdom ID parameters without spaces: diplomacy.legitimize_rebel_kingdom [Kingdom1]";
//        //    }
//        //    string b = strings[0].ToLower();
//        //    Kingdom kingdom1 = null;
//        //    foreach (Kingdom kingdom in KingdomExtensions.AllActiveKingdoms)
//        //    {
//        //        string a = kingdom.Name.ToString().ToLower().Replace(" ", "");
//        //        if (a == b)
//        //        {
//        //            kingdom1 = kingdom;
//        //        }
//        //    }
//        //    if (kingdom1 == null)
//        //    {
//        //        return "Could not find required kingdom!";
//        //    }
//        //    if (!kingdom1.IsRebelKingdom())
//        //    {
//        //        return "Specified kingdom is not a rebel kingdom!";
//        //    }
//        //    Func<RebelFaction, bool> <> 9__0;
//        //    Func<RebelFaction, bool> <> 9__1;
//        //    foreach (KeyValuePair<Kingdom, List<RebelFaction>> keyValuePair in RebelFactionManager.Instance.RebelFactions)
//        //    {
//        //        if (keyValuePair.Value != null)
//        //        {
//        //            IEnumerable<RebelFaction> value = keyValuePair.Value;
//        //            Func<RebelFaction, bool> predicate;
//        //            if ((predicate = <> 9__0) == null)
//        //            {
//        //                predicate = (<> 9__0 = ((RebelFaction f) => f.RebelKingdom == kingdom1));
//        //            }
//        //            if (value.Any(predicate))
//        //            {
//        //                Dictionary<Kingdom, List<RebelFaction>> rebelFactions = RebelFactionManager.Instance.RebelFactions;
//        //                Kingdom key = keyValuePair.Key;
//        //                IEnumerable<RebelFaction> value2 = keyValuePair.Value;
//        //                Func<RebelFaction, bool> predicate2;
//        //                if ((predicate2 = <> 9__1) == null)
//        //                {
//        //                    predicate2 = (<> 9__1 = ((RebelFaction f) => f.RebelKingdom != kingdom1));
//        //                }
//        //                rebelFactions[key] = value2.Where(predicate2).ToList<RebelFaction>();
//        //            }
//        //        }
//        //    }
//        //    if (RebelFactionManager.Instance.DeadRebelKingdoms.Contains(kingdom1))
//        //    {
//        //        RebelFactionManager.Instance.DeadRebelKingdoms.Remove(kingdom1);
//        //    }
//        //    return string.Format("{0} is no longer a rebel kingdom!", kingdom1.Name);
//        //}
//    }
//}
